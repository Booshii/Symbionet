<template>
    <div class="table-container">
        <form @submit.prevent="emitCompany">
            <!-- Name des Unternehmens -->
            <div class="form-group">
                <label for="companyname">Name des Unternehmens</label>
                <input type="text" id="companyname" v-model="createdCompany.name" required />
            </div>

            <!-- Dropdown für Branchen -->
            <div class="form-group">
                <label>Branche auswählen</label>
                <DropDownSelect
                    :options="dropdownOptions"
                    label="DropdownSelect"
                    :placeholder="'Branche auswählen'"
                    @update:selectedItem="handleSelect"
                />
            </div>

            <!-- Adresse -->
            <div class="form-group">
                <label for="street">Adresse-Straße</label>
                <input type="text" id="street" v-model="createdCompany.street" required />
            </div>
            <div class="form-group">
                <label for="streetnumber">Hausnummer</label>
                <input type="text" id="streetnumber" v-model="createdCompany.streetNumber" required />
            </div>
            <div class="form-group">
                <label for="postalcode">Postleitzahl</label>
                <input type="text" id="postalcode" v-model="createdCompany.postalcode" required />
            </div>
            <div class="form-group">
                <label for="city">Ort</label>
                <input type="text" id="city" v-model="createdCompany.city" required />
            </div>

            <!-- Kontaktinformationen -->
            <div class="form-group">
                <label for="contact-person">Kontaktperson</label>
                <input type="text" id="contact-person" v-model="createdCompany.contactPerson" />
            </div>
            <div class="form-group">
                <label for="contact-email">Kontakt-E-Mail</label>
                <input type="email" id="contact-email" v-model="createdCompany.contactEmail" />
            </div>
            <div class="form-group">
                <label for="contact-tel">Kontakt-Telefon</label>
                <input type="tel" id="contact-tel" v-model="createdCompany.contactTel" />
            </div>
            <div class="form-group">
                <label for="website-link">Webseite</label>
                <input type="text" id="website-link" v-model="createdCompany.websiteLink" />
            </div>

            <!-- Koordinaten -->
            <div class="form-group">
                <label for="longitude">Längengrad</label>
                <input type="text" id="longitude" v-model="createdCompany.longitude" />
            </div>
            <div class="form-group">
                <label for="latitude">Breitengrad</label>
                <input type="text" id="latitude" v-model="createdCompany.latitude" />
            </div>

            <!-- Button -->
            <button type="submit" class="submit-button">Unternehmen erstellen</button>
        </form>
    </div>
</template>

<script setup>
import { ref, defineEmits, onMounted } from 'vue'; 
import DropDownSelect from '../atoms/DropDownSelect.vue'; 

const props = defineProps({
    industries: { type: Array, required: true }
});

const selectedIndustryId = ref('');
const dropdownOptions = ref([...props.industries]);

const createdCompany = ref({
    name: '',
    street: '',
    streetNumber: '',
    postalcode: '',
    city: '',
    contactPerson: '',
    contactEmail: '',
    contactTel: '', 
    latitude: '',
    longitude: '',
    websiteLink: '', 
});

const emit = defineEmits(['add-company']);

onMounted(() => {
    console.log(props.industries);
});

function emitCompany(){
    emit('add-company', createdCompany.value, selectedIndustryId.value);
}

function handleSelect({selectedItem}){
    console.log(selectedItem);
    selectedIndustryId.value = selectedItem.id; 
};
</script>

<style scoped>
/* Container */
.table-container {
    display: flex;
    flex-direction: column;
    gap: 10px;
    width: 100%;
    background-color: #ffffff;
    border-radius: 15px;
    border: 1px solid #e1e8ed;
    padding: 20px;
    margin-top: 10px; /* Abstand nach oben */
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

/* Form Styling */
form {
    display: flex;
    flex-direction: column;
    gap: 15px;
}

/* Eingabefelder */
input, select {
    width: 100%;
    padding: 10px;
    border: 1px solid grey;
    border-radius: 6px; /* Weniger abgerundete Ecken */
    font-size: 14px;
    background-color: white;
    color: black;
    outline: none;
}

/* Kein grüner Rand */
input:focus, select:focus {
    border-color: grey;
    box-shadow: none;
}

/* Labels */
label {
    font-weight: bold;
    color: black;
    margin-bottom: 5px;
}

/* Button */
.submit-button {
    background-color: #2a9d8f;
    color: white;
    border: none;
    padding: 10px;
    border-radius: 6px; /* Weniger abgerundet */
    cursor: pointer;
    font-size: 16px;
    margin-top: 10px;
}

.submit-button:hover {
    background-color: #003366;
}
</style>
