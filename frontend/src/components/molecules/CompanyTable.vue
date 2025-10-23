<!-- 
    this component imports the companies from the backend and displays them in a table 
    with a edit button you can edit the data in the table 
 -->

<template>
    <div class="table-container">
        <!-- <Searchbar :placeholder="'Unternehmen eingeben'" :label="'Unternehmen suchen'" @update:search="handleSearchQueryName"/>
        <Searchbar :placeholder="'ID des Unternehmens eingeben'" :label="'Unternehmen suchen'" @update:search="handleSearchQueryId"/>
        <CompanyTable :headers="headers" :rows="filteredCompanies" @save-company="handleChangedCompany"/>
         -->
         <Popup v-if="showPopup === true" @confirm="handleConfirmDelete" @cancel="handleCancelDelete">
            Wollen Sie das ausgewählte Unternehmen löschen?
        </Popup>
        <table>
            <thead>
                <tr>
                    <th v-for="(head, index) in headers" :key="index">
                    {{ head }}
                    </th>
                </tr>
            </thead>
            <tbody>
                <template v-for="company in rows" :key="company.id">
                    <!-- Row with conditional styling -->
                  <tr :class="{ leftborder: showEditCompanyRowId === company.id }">
                    <td class="company-id">
                      {{ company.id }}
                    </td>
                    <td>
                        <span class="company-name">{{ company.name }}</span> <br />
                        <span> {{ company.industryName }}</span>
                    </td>
                    <td colspan="2">
                        <span>{{ company.street }} {{ company.streetNumber }} </span>
                        <br />
                        <span>{{ company.postalcode }} {{ company.city }}</span>
                    </td>
                    <td>
                        <span>{{ company.contactPerson }}</span>
                    </td>
                    <td>
                        <span>{{ company.contactEmail }} </span>
                        <br />
                        <span>{{ company.contactTel }} </span>
                    </td>
                    <td>
                        <span>{{ company.longitude }} </span>
                        <br />
                        <span>{{ company.latitude }} </span>
                    </td>
                    <td>
                      <button v-if = "showEditCompanyRowId=== company.id" @click="saveChanges(company.id, company.industryId)" class="save-button"> Speichern </button>
                      <button v-else @click="handleEditCompany(company)" class="edit-button"> Bearbeiten </button>

                    </td>
                    </tr>
                    <transition name="slide-fade">
                        <tr v-if="showEditCompanyRowId === company.id" id="edit-company-row">
                          <td>
                            <label for="company-id">Id</label> <br />
                            <input id="company-id" type="text" v-model="editCompany.id" />
                            <br/>
                        
                          </td>
                          <td>
                            <label for="company-name">Unternehmensname</label> <br />
                            <input type="text" id="company-name" v-model="editCompany.name" />
                            <br/> 
                            <label for="industry-name">Branche</label> <br/>
                            <DropDownSelectTiny 
                                :options="industries"
                                :label="'industryDropdownSelect'"
                                :placeholder="company.industryName "
                                @update:selectedItem="handleSelect"
                            />
                          
                          </td>
                            <td>
                                <label for="company-street">Straße</label> <br />
                                <input type="text" id="company-street" v-model="editCompany.street" />
                                <br />
                                <label for="company-city">City</label> <br />
                                <input type="text" id="company-city" v-model="editCompany.city" />
                            </td>
                            <td>
                                <label for="CompanyStreetNumber">Hausnummer</label> <br />
                                <input id="CompanyStreetNumber" type="text" class="short-input" v-model="editCompany.streetNumber" />
                                <br />
                                <label for="CompanyPostal">PLZ</label> <br />
                                <input id="CompanyPostal" type="text" class="short-input" v-model="editCompany.postalcode" />
                                <br />
                            </td>
                            <td>
                                <label for="CompanyContactPerson">Kontaktperson</label> <br />
                                <input id="CompanyContactPerson" type="text" v-model="editCompany.contactPerson" />
                                <br />
                            </td>
                            <td>
                                <label for="CompanyContactEmail">Email</label> <br />
                                <input id="CompanyContactEmail" type="text" v-model="editCompany.contactEmail" />
                                <br />
                                <label for="CompanyTel">Tel</label> <br />
                                <input id="CompanyTel" type="text" v-model="editCompany.contactTel" />
                            </td>
                            <td>
                                <label for="CompanyLongitude">Longitude</label> <br />
                                <input id="CompanyLongitude" type="text" v-model="editCompany.longitude" />
                                <br />
                                <label for="CompanyLatitude">Latitude</label> <br />
                                <input id="CompanyLatitude" type="text" v-model="editCompany.latitude" />
                            </td>
                            <td>
                                <Button type="default" variant="delete" @click="handleDeleteRequest(editSdgTarget)"> Löschen </Button>
                            </td>
                           
                      </tr>
                    </transition>
                </template>
            </tbody>
        </table>
    </div>
</template>

<script setup>
    import { ref, defineProps, reactive, onMounted } from 'vue'; 

    import Button from '../atoms/Button.vue';
    import Popup from '../atoms/Popup.vue';
    import DropDownSelectTiny from '../atoms/DropDownSelectTiny.vue';

    import axiosInstance from '@/axios';

    //Props
    const props = defineProps({
        rows: {
            type: Array,
            required: false,
        },
        headers: {
          type: Array,
          require: false,
        }
    });

    onMounted(() => {
        getIndustries();
    });

    
    const showPopup = ref(false);
    const showEditCompanyRowId = ref(false);
    const industries = ref(null);
    const selectedIndustry = ref(null);

    const editCompany = reactive({
        name: '',
        street: '',
        streetNumber: '',
        postalcode: '',
        city: '',
        industryId: '',
        contactPerson: '',
        contactEmail: '',
        contactTel: '',
        latitude: '',
        longitude: '',
    });

    const emit = defineEmits(['update-company']);

    /******************************************/
    /**************** Buttons *****************/
    /******************************************/
    function handleEditCompany(company){
      Object.assign(editCompany, company);
      // Setze den aktuell zugeordneten Industry-Eintrag
        const matchedIndustry = industries.value.find(ind => ind.id === company.industryId);
        if (matchedIndustry) {
            selectedIndustry.value = matchedIndustry;
        } else {
            selectedIndustry.value = null;
        }
      showEditCompanyRowId.value = company.id;
    }
    function saveChanges(companyId, industryId) {
        emit('update-company', { ...editCompany }, companyId, industryId);
        showEditCompanyRowId.value = false;
    }
        // function which displays a popup with a security prompt
        function handleDeleteRequest(){
        showPopup.value = true; 
    }
    function handleConfirmDelete(){
        emit('delete-company', editCompany.id);
        showPopup.value = false; 
    }
    function handleCancelDelete(){
        showPopup.value = false; 
    }


    async function getIndustries() {
        try {
            const response = await axiosInstance.get('/industry');
            console.log(response);
            industries.value = response.data;
        } catch (error) {
            console.error('Fehler beim Abrufen der Branchen:', error);
        }
    }

    function handleSelect({ selectedItem }) {
        if (selectedItem && selectedItem.id !== undefined) {
            editCompany.industryId = selectedItem.id;
            selectedIndustry = selectedItem.name; // optional für Anzeige
        } else {
            editCompany.industryId = null;
            editCompany.industryName = null;
        }
    }

</script>

<style scoped>
 .table-container {
    display: flex;
    flex-direction: column;
    gap: 10px;
    width: 100%;
}

/* Suchleisten und Tabelle gleich hoch */
.search-input, th, td {
  line-height: 30px;
}

/* Tabelle */
table {
    border-collapse: collapse;
    width: 100%;
    font-family: Arial, sans-serif;
    background-color: #f9f9f9;
    border-radius: 20px; /* Abrundung der Ecken */
    border: solid; 
    overflow: hidden; /* Abrundung bleibt erhalten */
    border: 1px solid #e0e0e0;
}

/* Tabellenkopf */
th {
    background-color: white; 
    /* background-color: #004d99; */
    color: black; 
    text-align: left;
    border-bottom: 2px solid #003366;
    padding: 0 0 0 20px; 
    height: 50px;
}

/* Alternierende Zeilenfarben */
tbody tr:nth-child(even) {
    background-color: #f2f2f2;
}

tbody tr:nth-child(odd) {
    background-color: #ffffff;
}

/* Zellen */
td {

  padding-left: 24px; 
    border-bottom: 1px solid #e0e0e0;
    color: #333;
    
}

/* Abrundung für Suchleisten */
.search-input {
    display: flex;
    align-items: center;
    border: 1px solid #ccc;
    border-radius: 10px;
    padding: 5px 10px;
    background-color: white;
}

/* Eingabefelder in der Suchleiste */
.search-input input {
    border: none;
    outline: none;
    flex-grow: 1;
    font-size: 14px;
    height: 30px;
}

/* Buttons */
.edit-button, .save-button {
    border: none;
    padding: 8px 12px;
    cursor: pointer;
    border-radius: 5px;
    transition: background-color 0.3s;
}

.edit-button {
    background-color: #2a9d8f;
    color: white;
}

.edit-button:hover {
    background-color: #21867a;
}

.save-button {
    background-color: #004d99;
    color: white;
}

.save-button:hover {
    background-color: #003366;
}
#company-id{
  width: 20px
}
  </style>