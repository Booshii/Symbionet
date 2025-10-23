    <template>
    <div>
        <h1>Datei für mehrere {{dataCategory}} hochladen</h1>
        <input type="file" @change="handleFileSelection" accept=".json">
        
        <!-- Fehler- und Erfolgsnachrichten -->
        <p v-if="errorMessage" style="color: red;">{{ errorMessage }}</p>
        <p v-if="uploadSuccess" style="color: green;">Datei erfolgreich hochgeladen</p>

        <!-- Upload-Button, aktiviert nur, wenn eine Datei ausgewählt wurde -->
        <Button :disabled="!selectedFile" type="submit" variant="default" @click="emitFile"> Datei hochladen </Button>
    </div>
</template>

<script setup>
    import { ref, defineEmits } from 'vue'; 

    import Button from '../atoms/Button.vue';


    const errorMessage = ref('');
    const selectedFile = ref(null); 
    

    const props = defineProps({
        dataCategory: String,
				uploadSuccess: Boolean,
				uploadError: String,
    })

    const emit = defineEmits(['file-uploaded']); 

    function handleFileSelection(event) {
        selectedFile.value = event.target.files[0]; 
        errorMessage.value = ''; // Vorherige Fehlermeldungen zurücksetzen
				// hier if ob file wirklich json file 
				if (selectedFile.value && !selectedFile.value.name.endsWith('.json')){
					errorMessage.value = "Ungültiges Dateiformat. Bitte wählen Sie eine JSON-Datei.";
					selectedFile.value = null;
				}
    }

		function emitFile() {
            console.log("bin drin"); 
			if (!selectedFile.value) {
				errorMessage.value = "Bitte wählen Sie eine Datei aus.";
				return;
			}
			
			const reader = new FileReader(); 
            console.log("bin drin 2"); 
			reader.onload = (e) => {
				try{
					const jsonData = JSON.parse(e.target.result);
                    emit("file-uploaded", jsonData);
				} catch (error) {
					errorMessage.value = "Fehler: Ungültige JSON-Datei"; 
                    console.log("bin drin 2"); 
				}
			};
			reader.readAsText(selectedFile.value); 
		



		// es sollte dem User mitgeteilt werden ob er überhaupt eine datei ausgwählt hat
		// ob es eine json datei ist
		// ob die json eine Falsche Struktur hat 
		// ob es einen Fehler beim Upload gab 



        // reader.onload = async (e) => {
        //     try {
        //         const jsonData = JSON.parse(e.target.result);
        //         emit("file-uploaded", jsonData); 

							//
                        
            //         successMessage.value = 'Alle Daten wurden erfolgreich hochgeladen'; 
            //         emit('updateJsonData', data); 
            //     } else {
            //         errorMessage.value = 'Die Datei sollte ein Array von Unternehmen enthalten.';
            //     }
            // } catch (error){
            //     errorMessage.value = 'Fehler beim Verarbeiten der Datei. Bitte stellen Sie sicher, dass die Datei im JSON-Format vorliegt.';
            // }
        // }
        // reader.readAsText(selectedFile.value); 
    }
        



	
</script>
