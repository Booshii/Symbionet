<script setup>
    import axiosInstance from 'axios'; 
    import { ref, onMounted, onUpdated } from 'vue';

    import EditCompaniesTable from '../molecules/CompanyTable.vue';

    const companies = ref([]);
            
    async function getCompanies() {
        try {
            const response = await axiosInstance.get('/company');
            companies.value = response.data;
            console.log(companies.value);
        } catch (error) {
            console.error('Error fetching people:', error);
        }
    }

    
    const formData = ref({
        
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
    });

    //**************************************/
    //*********** Table Company ***********/
    //**************************************/

    //**************************************/
    //************* Add Company ************/
    //**************************************/

    async function createCompany() {
        
        try {
            const response = await axiosInstance.post('/company', formData.value);
            console.log('Success:', response.data);
        } catch (error) {
            console.error('Error:', error);
        }
    };
    // *********** File upload **********
    const file = ref(null); 
    const companiesFromFile = ref([]); 
    function handleFileUpload(event) {
        file.value = event.target.files[0];
        if (file) {
            const reader = new FileReader(); 
            reader.onload = function(e) {
                try {
                    const json = JSON.parse(e.target.result); 

                } catch (error) {
                    console.error('Fehler beim Parsen der JSON-Datei', error); 
                }
            };
            reader.readAsText(file);
        }
    }

    function uploadData() {
        if(companiesFromFile.value.length > 0 ) {
            axiosInstance.post('/company', { companies: companies.value})
                .then(function (response) {
                    console.log('Daten erfolgreich hochgeladen:', response.data);
                })
                .catch(function (error) {
                    console.error('Fehler beim Hochladen der Daten:', error);
                });
        } else {
            console.error('Keine Daten zum Hochladen vorhanden.');
        
        }
    }

    // edit Company
    const editingIndex = ref(null); // Speichert den Index der aktuell bearbeiteten Zeile
    const editCache = ref({}); // Speichert die Daten der bearbeiteten Zeile
    
    function editCompany(index) {
        editingIndex.value = index; // Setzt den bearbeiteten Index
        editCache.value = { ...companies.value[index] }; // Kopiert die Daten der bearbeiteten Zeile in editCache
    }
    //{ ...companies.value[index] }: Diese Syntax erstellt ein neues Objekt, das alle Eigenschaften des ursprünglichen Firmenobjekts enthält.


    function isEditing(index) {
        return editingIndex.value === index; // Überprüft, ob der aktuelle Index bearbeitet wird
    }

    async function updateCompany(){
        const company = editCache.value;
        console.log('Saving company:', company); 
        try {
            const response = await axiosInstance.put('/company/${company.id}', company);
            companies.value[index] = { ...company }; // Aktualisiert die Hauptliste mit den bearbeiteten Daten
            editingIndex.value = null; // Beendet den Bearbeitungsmodus
        } catch (error) {
            console.error('Error update company', error); 
        }
    }
    //***************breite *************** */
    const breite = ref(window.innerWidth);

    function updateWindowSize(){
        breite.value = window.innerWidth; 
    }; 
    onMounted(function() {
        window.addEventListener('resize', updateWindowSize); 
        getCompanies(); 
    });
    // onUpdated(() => {
    //     emitWidth(); 
    // }); 
    
</script>

<template>
    <div class="container">
           
        <h1 id="breite">{{ breite }}</h1>

        <EditCompaniesTable />

        <h1 class="section-title">Unternehmen erstellen</h1>

        <form @submit.prevent="createCompany" class="create-company-form">
        <div class="form-group">
            <label for="companyname">Name des Unternehmens</label>
            <input type="text" id="companyname" v-model="formData.name" required />
        </div>
        <div class="form-group">
            <label for="street">Adresse-Straße</label>
            <input type="text" id="street" v-model="formData.street" required />
        </div>
        <div class="form-group">
            <label for="streetnumber">Hausnummer</label>
            <input type="text" id="streetnumber" v-model="formData.streetNumber" required />
        </div>
        <div class="form-group">
            <label for="postalcode">Postleitzahl</label>
            <input type="text" id="postalcode" v-model="formData.postalcode" required />
        </div>
        <div class="form-group">
            <label for="city">Ort</label>
            <input type="text" id="city" v-model="formData.city" required />
        </div>
        <div class="form-group">
            <label for="contact-person">Kontaktperson</label>
            <input type="text" id="contact-person" v-model="formData.contactPerson" required />
        </div>
        <div class="form-group">
            <label for="contact-email">KontaktEmail</label>
            <input type="email" id="contact-email" v-model="formData.contactEmail" required />
        </div>
        <div class="form-group">
            <label for="contact-tel">Kontakttelefon</label>
            <input type="tel" id="contact-tel" v-model="formData.contactTel" required />
        </div>
        <div class="form-group">
            <label for="longitude">Längengrad</label>
            <input type="text" id="longitude" v-model="formData.longitude" required />
        </div>
        <div class="form-group">
            <label for="latitude">Breitengrad</label>
            <input type="text" id="latitude" v-model="formData.latitude" required />
        </div>

        <button type="submit" class="create-button">Unternehmen erstellen</button>
        </form>

        <div class="fileupload">
        <h2 class="section-title">Datei Upload</h2>

        <input type="file" @change="handleFileUpload" />
        <button @click="uploadData" :disabled="!file" class="upload-button">
            Daten hochladen
        </button>
        </div>
    </div>

    
</template>


<style scoped>
    .container {
        margin: 20px;
    }

    .section-title {
        font-size: 24px;
        font-weight: bold;
        color: #004d99;
        margin-bottom: 20px;
        border-bottom: 2px solid #2a9d8f;
        padding-bottom: 10px;
    }

    /* .create-company-form {
        display: flex;
        flex-direction: column;
        gap: 15px;
        background-color: #f9f9f9;
        padding: 20px;
        border: 2px solid #e0e0e0;
        border-radius: 10px;
        box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.1);
    } */

    .form-group {
    display: flex;
    flex-direction: column;
    gap: 5px;
    }

    label {
    font-weight: bold;
    color: #004d99;
    }

    input {
    padding: 10px;
    border: 1px solid #ccc;
    border-radius: 5px;
    transition: border-color 0.3s;
    }

    input:focus {
    border-color: #2a9d8f;
    outline: none;
    }

    .create-button {
    background-color: #2a9d8f;
    color: white;
    padding: 10px 15px;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    transition: background-color 0.3s;
    }

    .create-button:hover {
    background-color: #21867a;
    }

    .fileupload {
    margin-top: 30px;
    }

    .upload-button {
    background-color: #004d99;
    color: white;
    padding: 10px 15px;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    margin-top: 10px;
    transition: background-color 0.3s;
    }

    .upload-button:hover {
    background-color: #003366;
    }

    input[type="file"] {
    margin-top: 10px;
    }
</style>