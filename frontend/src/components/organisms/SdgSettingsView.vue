<template>
	<div class="container-sdg-settings-view">
			<div class="searchbars">
					<Searchbar :placeholder="'Titel eingeben'" :label="'nach Titel suchen'" @update-search="handleSearchQueryCompany"/>
					<Searchbar :placeholder="'ID des Unternehmens eingeben'" :label="'Unternehmen suchen'" @update-search="handleSearchQueryId"/>
			</div> 
			<sdgTargetTable v-if="sdgTypes" :headers="headers" :rows="filteredSdgTargets" :sdgTypes="sdgTypes" @save-sdg-target="handleChangedSdgTarget" @delete-sdg-target="handleDeleteSdgTarget"/>
			<h1 class="section-title">SDG-Maßnahme erstellen</h1>
			<SdgTargetFormCreate v-if="companies && sdgTypesSimple" :companies="companies" :sdgTypes="sdgTypesSimple" @add-sdgTarget="handleAddSdgTarget"/>
			<FileUpload
				:dataCategory="'SDG-Ziele'"
				:uploadSuccess="uploadSuccess"
				:uploadError="uploadError"
				@file-uploaded="handleFileUpload"
			/>
	</div>
</template>

<script setup>
	import { ref, computed, onMounted} from 'vue';
	import axiosInstance from "@/axios";

	import Searchbar from '../atoms/Searchbar.vue'; 
	import SdgTargetTable from '../molecules/SdgTargetTable.vue'; 
	import SdgTargetFormCreate from '../molecules/SdgTargetFormCreate.vue'; 
	import FileUpload from '../molecules/FileUpload.vue'; 
	import { useAuthStore } from '@/store/authStore';

	// Services importieren
	import { uploadJsonData } from '@/services/uploadService';
	import { validateJson } from '@/services/validateJsonService';

	const authStore = useAuthStore();

	const sdgTargets = ref(null); 
	const companies = ref(null); 
	const sdgTypes = ref(null); 
	const sdgTypesSimple = ref(null); 

	const searchQueryCompany = ref('');
	const searchQueryId = ref('');
	const activeFilter = ref('company'); 

	// for FileUpload
	const uploadSuccess = ref(false);
	const uploadError = ref("");

	const headers = ['ID', 'Titel/Unternehmen', 'Beschreibung', 'Umgesetzt', 'Veröffentlicht', '']; 

	onMounted(() => {
			getSdgTargets();
			getCompanies();
			getSdgTypes(); 
			getSdgTypesSimple();
	})

	//**************************************/
	//*********** Get SdgTargets ***********/
	//**************************************/
	async function getSdgTargets() {
		const params = {}
		try {
			let response; 
			if(authStore.isAdmin){
				params.isDecsending = false;
				response = await axiosInstance.get('/sdg');
			} else if (authStore.companyId) {
				params.companyId = authStore.companyId;
				response = await axiosInstance.get('/sdg', { params });
			} else {
				console.warn('Weder Admin noch gültige CompanyId vorhanden');
				return;
			}
			
			sdgTargets.value = response.data;
			console.log(response);
		} catch (error) {
			console.error('Error fetching people:', error);
		}
	}

	//**************************************/
	//*********** Add SdgTarget ************/
	//**************************************/
	async function handleAddSdgTarget(createdSdgTarget, companyId) {
			console.log("createdSdgObjekt: ", createdSdgTarget);
			console.log(companyId);
			try {
					
					const response = await axiosInstance.post(`/sdg/${companyId}`, createdSdgTarget);
					console.log('Success:', response.data);
					// sdgTargets.value.push(response.data);
					await getSdgTargets();
			} catch (error) {
					if (error.response) {
							console.error('Error response data:', error.response.data); // Zeigt die Antwort des Servers
							console.error('Error status:', error.response.status); // Zeigt den Statuscode
							console.error('Error headers:', error.response.headers); // Zeigt die Header der Antwort
					} else if (error.request) {
							console.error('Error request:', error.request); // Zeigt das Request-Objekt
					} else {
							console.error('General Error:', error.message); // Zeigt allgemeine Fehler
					}
			}
	}

	//**************************************/
	//********** Update SdgTarget **********/
	//**************************************/
	async function handleChangedSdgTarget(editedSdgTarget, sdgTargetId){ 
			try {
					console.log(editedSdgTarget); 
					console.log(sdgTargetId); 
					const response = await axiosInstance.put(`/sdg/${sdgTargetId}`, editedSdgTarget);
					console.log('Success:', response.data);

					// add edited company to local list after the backend call was successfully 
					const index = sdgTargets.value.findIndex((sdgTarget) => sdgTarget.id === editedSdgTarget.id);

					if(index !== -1){
							sdgTargets.value[index] = { ...sdgTargets.value[index], ...response.data }; 
							// nur ein Array mit zahlen 
					}

			} catch (error) {
					console.error('Error:', error);
			}      
	};
	//**************************************/
	//********** Delete SdgTarget **********/
	//**************************************/

	async function handleDeleteSdgTarget(sdgTargetId) {
			try {
					console.log(sdgTargetId);
					const response = await axiosInstance.delete(`/sdg/${sdgTargetId}`);
					sdgTargets.value = sdgTargets.value.filter(item => item.id !== sdgTargetId);
					console.log(response);
			} catch (error) {
					console.error('Error deleting sdgTarget')
			}

	}

	//**************************************/
	//*********** Get Companies ************/
	//**************************************/
	async function getCompanies() {
			try {
					const response = await axiosInstance.get('/company');
					companies.value = response.data;
					console.log(response);
			} catch (error) {
					console.error('Error fetching companies:', error);
			}
	}

	//**************************************/
	//*********** Get SdgTypesSimple *************/
	//**************************************/
	async function getSdgTypesSimple() {
		try {
			const response = await axiosInstance.get('/sdgtype/simple');
			sdgTypesSimple.value = response.data;
			console.log(response);
		} catch (error) {
			console.error('Error fetching people:', error);
		}
	}

	//**************************************/
	//*********** Get SdgTypes *************/
	//**************************************/
	async function getSdgTypes() {
		try {
			const response = await axiosInstance.get('/sdgtype');
			sdgTypes.value = response.data;
			console.log(response);
		} catch (error) {
			console.error('Error fetching people:', error);
		}
	}


	/******************************************/
	/**************** filters *****************/
	/******************************************/

	const filteredSdgTargets = computed(() => {
			if(activeFilter.value === 'company' && searchQueryCompany.value) {
					return sdgTargets.value.filter((sdgTarget) =>
							sdgTarget.companyName.toLowerCase().includes(searchQueryCompany.value.toLowerCase()) ||
							(sdgTarget.title && sdgTarget.title.toLowerCase().includes(searchQueryCompany.value.toLocaleLowerCase()))
					);          
			} else if (activeFilter.value === 'id' && searchQueryId.value){
					return sdgTargets.value.filter((sdgTarget) => 
							sdgTarget.id.toString().includes(searchQueryId.value)
					);
			}
			return sdgTargets.value
	});
	
	//*********** Searchbar name ***********/

	const handleSearchQueryCompany = (query) => {
			console.log(query);
			searchQueryCompany.value = query;
			activeFilter.value = 'company';
	}
	//************ Searchbar id ************/
	const handleSearchQueryId = (query) => {
			searchQueryId.value = query;
			activeFilter.value = 'id'; 
	}

	/******************************************/
	/************* handle Upload **************/
	/******************************************/

	async function handleFileUpload(jsonData) {
		uploadSuccess.value = false;
    uploadError.value = ""; // Zurücksetzen der Fehlernachricht

		if (!validateJson(jsonData, "sdg")){
			console.log(sdgResult); 
			return; 
		}

		const response = await uploadJsonData(jsonData, "/sdg/fileUpload"); 
			
		if (response.success){
			uploadSuccess.value = true;
		} else {
			uploadError.value = "Fehler beim Hochladen der Datei: " + response.error;
		}
	}

	// *********** Testdatein ****************

//     const options = ref([
//         { id: 1, name: 'Keine Ahmut' },
//         { id: 2, name: 'Kein Hunger' },
//         { id: 3, name: 'Gesundheit und Wohlergehen' },
//         { id: 4, name: 'Hochwertige Bildung' },
//         { id: 5, name: 'Geschlechtergleichheit' },
//         { id: 6, name: 'Sauberes Wasser' },
//         { id: 7, name: 'Bezahlbare und saubere Energie' },
//         { id: 8, name: 'Menschenwürdige Arbeit und Wirtschaftswachstum' },
//         { id: 9, name: 'Industrie, Innovation und Infrastruktur' },
//         { id: 10, name: 'Weniger Ungleichheiten' },
//         { id: 11, name: 'Nachhaltige Städte und Gemeinden' },
//         { id: 12, name: 'Nachhaltige/r Konsum und Produktion' },
//         { id: 13, name: 'Maßnahmen zum Klimaschutz' },
//         { id: 14, name: 'Leben unter Wasser' },
//         { id: 15, name: 'Leben an Land' },
//         { id: 16, name: 'Frieden, Gerechtigkeit und starke Institutionen' },
//         { id: 17, name: 'Partnerschaften zur Erreichung der Ziele' }
// ]); 
	
//     sdgTargets.value = [
//         {
//             id: '1',
//             title: 'Kaffekanne',
//             company: 'cool GmbH',
//             description: 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.',
//             isDone: 'done',
//             pictures: 'example'

//         },
//         {
//             id: '2',
//             title: 'test',
//             company: 'ste GmbH',
//             description: 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.',
//             isDone: 'done',
//             pictures: 'example'

//         },
//         {
//             id: '3',
//             title: 'testBerg',
//             company: 'cool undnoch mehr Gbr',
//             description: 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.',
//             isDone: 'done',
//             pictures: 'example'

//         },
//     ]

	
</script>

<style scoped>
	.container-sdg-settings-view {
			margin: 12px;
	}
	.searchbars {
					display: flex; 
					flex-direction: row; 
	}
	.section-title {
			font-size: 24px;
			font-weight: bold;
			font-family: ui-sans-serif, sans-serif;
			color: black;
			margin-top: 40px; 
			/* border-bottom: 2px solid #2a9d8f; */
			padding-bottom: 10px;
	}

</style>