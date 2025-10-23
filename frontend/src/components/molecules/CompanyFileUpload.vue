<template>
    <div>
        <input type="file" @change="handleFileChange" accept=".json" />
        <!-- Errormessage for validation -->
         <div v-if="errorMessage">
            <p style="color: red"> {{ errorMessage }}</p>
         </div>

        <div v-if="jsonData">
            <h3>Datei für mehrere Maßnahmen hochladen</h3>
            <pre>{{  jsonData }}</pre>
        </div>

        <Button type="default" variant="default" :disabled="!selectedFile" class="upload-button" @click="emitJsonFile"> Daten hochladen </Button>
        <!-- <button @click="emitJsonFile" :disabled="!selectedFile" class="upload-button">
            Daten hochladen
        </button> -->
    </div>
</template>

<script setup>
    import { ref } from 'vue'; 
    import Button from '../atoms/Button.vue'

    const jsonData = ref(null); // Json Content
    const errorMessage = ref(null); 
    const selectedFile = ref(null ) 


    const emit = defineEmits(['add-companiesFromFileUpload']);
    function emitJsonFile(){
        emit('add-companiesFromFileUpload', jsonData.value);
    }
    // method to validate and show the content of the choosen file 
    // 
    function handleFileChange(event){
        
        const file = event.target.files[0]; 
        
        if(!file) {
            errorMessage.value = "Bitte wählen Sie eine Datei aus.";
            selectedFile.value = null; 
            return; 
        }

        if (file.type !== "application/json") {
            errorMessage.value = "falsches Dateiformat. Bitte wählen Sie eine .json Datei aus.";
            selectedFile.value = null; 
            return; 
        }

        selectedFile.value = file; 
        errorMessage.value = null; 

        // read json file
            
        const reader = new FileReader();
        
        // onFileLoad() is assigned to the file reader's onload event. 
        // onload event is triggered when the file reader has read the file completely
        reader.onload = onFileLoad; 
        
        // reads the file and triggers the onload event above
        reader.readAsText(selectedFile.value); 
    }
   
    // is called when the onload-event of the FileReader is triggered
    // this happens as soon as the file has been read completely
    // method for onload event processing
    function onFileLoad(event) {
        try {
            // parse content and save
            jsonData.value = JSON.parse(event.target.result);
            console.log("Hochgeladene JSON-Datei: ", jsonData.value); 

            // emitJsonFile(); 
        } catch (error) {
            console.error("Fehler beim Parsen der JSON-Datei: ", error); 
        }
    }

</script>

<style scoped>
    input {
    padding: 8px; /* Etwas mehr Padding für eine größere Klickfläche */
    border: 1px solid #ccc;
    border-radius: 8px; /* Runde Ecken für ein moderneres Design */
    font-size: 16px; /* Größere Schrift für bessere Lesbarkeit */
    transition: all 0.3s ease; /* Sanfter Übergang für Änderungen */
    box-shadow: inset 0 1px 2px rgba(0, 0, 0, 0.1); /* Leichter Schatten für Tiefe */
}

input:hover {
    border-color: #2a9d8f; /* Leichtes Farbspiel beim Hover */
    box-shadow: 0px 0px 5px rgba(42, 157, 143, 0.5); /* Leichter Außen-Schatten bei Hover */
}

input:focus {
    border-color: #2a9d8f; /* Farbliche Hervorhebung beim Fokus */
    outline: none; /* Standard-Umriss entfernen */
    box-shadow: 0px 0px 5px rgba(42, 157, 143, 0.7); /* Stärkerer Außen-Schatten im Fokus */
    background-color: #f0f8f7; /* Dezenter Hintergrundwechsel im Fokus */
}
</style>