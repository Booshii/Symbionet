<template>
    <div class="container-industry-table">
        <Popup v-if="showPopup === true" @confirm="handleConfirmDelete" @cancel="handleCancelDelete">
            Wollen Sie die ausgewählte Branche löschen?
        </Popup>
        <table>
            <thead>
                <tr>
                    <th>
                        ID
                    </th>
                    <th>
                        Titel
                    </th>
                    <th></th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="industry in rows" :key="industry.id">
                    <td>
                        {{ industry.id }}
                    </td>
                    <td v-if="showEditRowId === industry.id">
                        <!-- Input-Feld im Bearbeitungsmodus -->
                        <input v-model="editedIndustry.name" />
                    </td>
                    <td v-else>
                        <!-- Input-Feld im Anzeige-Modus-->
                        {{ industry.name }}
                    </td>
                    <td>
                        <!-- Button im Bearbeitungsmodus -->
                        <button v-if="showEditRowId === industry.id" @click="saveChanges(industry.id)" class="button" id="save">
                            Speichern
                        </button>
                        <!-- Button im Anzeige-Modus-->
                        <button v-else @click="handleEditIndustry(industry)" class="button" id="edit">
                            Bearbeiten
                        </button>
                    </td>
                    <td v-if ="showEditRowId === industry.id">
                        <button @click="handleDeleteRequest(industry.id)" class="button" id="delete">
                            Löschen
                        </button>
                    </td>
                </tr>
            </tbody>
        </table>

    </div>
</template>

<script setup>
    import { reactive, ref, defineEmits } from 'vue';
    import Popup from '../atoms/Popup.vue';

    //Props
    const props = defineProps({
        rows: {
            type: Array,
            required: false,
        },
    });
    const showPopup = ref(false);
    // const showEditRow = ref(false);
    const showEditRowId = ref(null);

    const editedIndustry = reactive({
        id: '', // leer
        name: '',
    })

    const emit = defineEmits(['delete-industry', 'update-industry']); 

    /******************************************/
    /**************** Buttons *****************/
    /******************************************/
    function handleEditIndustry(industry){
        Object.assign(editedIndustry, industry);
        showEditRowId.value = editedIndustry.id;
    }
    function saveChanges(industryId) {
        emit('update-industry', { ...editedIndustry}, industryId);
        showEditRowId.value = null; 
    }

    // muss noch umgeschrieben werden 
    // function deleteItem(id) {
    //     const index = industries.value.findIndex(industry => industry.id === id);
    //     if (index !== -1){
    //         industries.value.splice(index, 1);
    //     }
    // }

    // function which displays a popup with a security prompt
    function handleDeleteRequest(){
        showPopup.value = true; 
    }
    function handleConfirmDelete(){
        emit('delete-industry', editedIndustry.id);
        showPopup.value = false; 
    }
    function handleCancelDelete(){
        showPopup.value = false; 
    }


    // Dummy-Daten
    // const industries = ref([
    //     { id: 1, name: 'Technologie' },
    //     { id: 2, name: 'Gesundheit' },
    //     { id: 3, name: 'Finanzen' }
    // ]);
</script>

<style scoped>
  .container-industry-table {
    display: flex;
  }
  
  table {
    border-collapse: collapse;
    width: 100%;
    font-family: Arial, sans-serif;
    background-color: #f9f9f9;
    border: 1px solid #e0e0e0;
  }
  
  th {
    background-color: #004d99;
    color: white;
    padding: 10px;
    text-align: left;
    border-bottom: 2px solid #003366;
  }
  
  td {
    padding: 10px;
    border-bottom: 1px solid #e0e0e0;
    color: #333;
  }
    
  .button {
    color: white;
    border: none;
    padding: 5px 10px;
    cursor: pointer;
    border-radius: 5px;
    transition: background-color 0.3s;
  }
  
  .button:hover {
    background-color: #21867a;
  }
  
  #save {
    background-color: #004d99;
  }
  
  #edit{
    background-color: #2a9d8f;
  }
  #delete {
    background-color: red;
  }

</style>