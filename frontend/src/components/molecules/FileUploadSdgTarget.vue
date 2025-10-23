<script setup>
    import { ref, defineEmits } from 'vue'; 
    import axiosInstance from "@/axios";

    import Button from '../atoms/Button.vue';


    const errorMessage = ref('');
    const successMessage = ref('');
    const selectedFile = ref(null);
		const uploadedData = ref(null); 
    
    const apiEndpoint = '/sdg/fileUpload';

    const emit = defineEmits(['updateJsonData']); 

    function handleFileSelection(event) {
        selectedFile.value = event.target.files[0]; 
        errorMessage.value = ''; // Vorherige Fehlermeldungen zurücksetzen
        successMessage.value = ''; // Erfolgsnachricht zurücksetzen
				uploadedData.value = null; 

				if (!selectedFile.value.name.endsWith('json')) {
					errorMessage.value = "Bitte laden Sie eine gültige JSON-Datei hoch"; 
					selectedFile.value = null;
				}
    }

    async function handleFileUpload() {
        if (!selectedFile.value) {
            errorMessage.value = "Bitte wählen Sie eine Datei aus.";
            return;
        }

        const reader = new FileReader(); 
        reader.onload = async (e) => {
            try {
                const jsonData = JSON.parse(e.target.result);
								console.log("Gelesene JSON-Daten:", jsonData);
                
                if(!validateJson(jsonData)){
                    errorMessage.value = "Ungültige JSON-Struktur"; 
                    return;
                }

                await uploadJsonData(jsonData);
            
                
            } catch (error){
                errorMessage.value = 'Fehler beim Verarbeiten der Datei. Bitte stellen Sie sicher, dass die Datei im JSON-Format vorliegt.';
            }
        }
        reader.readAsText(selectedFile.value); 
    }

    function validateJson(data) {
			return Array.isArray(data) && data.every((entry) =>
        typeof entry.CompanyId === 'number' && // CompanyId muss eine Zahl sein
        Array.isArray(entry.CreateSdgTargetDtos) &&
        entry.CreateSdgTargetDtos.every((m) =>
            typeof m.Title === 'string' &&
            typeof m.Description === 'string' &&
            typeof m.IsDone === 'boolean' && // IsDone ist ein bool (true/false)
            Array.isArray(m.SdgTypeIds) &&
            m.SdgTypeIds.every(id => typeof id === 'number') // Jedes SdgTypeId muss eine Zahl sein
        )
    	);
    }

		async function uploadJsonData(jsonData) {
			try {
				const response = await axiosInstance.post(apiEndpoint, jsonData, {
					headers: {"Content-Type": "application/json"}
				});

				successMessage.value = "Datei erfolgreich hochgeladen"; 
				uploadedData.value = response.data; 

			} catch (error) {
				errorMessage.value = "Fehler beim Hochladen der Datei"; 
				console.error('Upload-Fehler:', error); 
			}
		}



        
    // async function uploadJsonData(company) {
    //     try {
    //         const response = await axios.post(apiEndpoint, company);
    //         console.log(`Daten für ${company.name} erfolgreich hochgeladen:`, response.data);
    //     } catch (error) {
    //         errorMessage.value = `Fehler beim Hochladen von ${company.name}.`;
    //         console.error('Upload-Fehler:', error);
    //     }
    // }


    // if (Array.isArray(data)) {
    //                 // Daten als Array verarbeiten und jedes Objekt einzeln hochladen
    //                 for (const company of data) {
    //                     await uploadJsonData(company);
    //                 }
                    
    //                 successMessage.value = 'Alle Daten wurden erfolgreich hochgeladen'; 
    //                 emit('updateJsonData', data); 
    //             } else {
    //                 errorMessage.value = 'Die Datei sollte ein Array von Unternehmen enthalten.';
    //             }



</script>

<template>
    <div>
        <h1>Datei für mehrere Maßnahmen hochladen</h1>
        <input type="file" @change="handleFileSelection" accept=".json">
        
        <!-- Fehler- und Erfolgsnachrichten -->
        <p v-if="errorMessage" style="color: red;">{{ errorMessage }}</p>
        <p v-if="successMessage" style="color: green;">{{ successMessage }}</p>

        <!-- Upload-Button, aktiviert nur, wenn eine Datei ausgewählt wurde -->
        <Button :disabled="!selectedFile" type="submit" variant="default" @click="handleFileUpload"> Datei hochladen </Button>
    
				<div v-if="uploadedData">
            <h2>Hochgeladene Daten:</h2>
            <pre>{{ JSON.stringify(uploadedData, null, 2) }}</pre>
        </div>
		</div>
</template>

<script scoped>
</script>
