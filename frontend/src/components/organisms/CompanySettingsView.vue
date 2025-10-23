
<template>
	<div class="container-company-settings-view">
			<div class="searchbars">
					<Searchbar :placeholder="'ID des Unternehmens eingeben'" :label="'Unternehmen suchen'" @update-search="handleSearchQueryId"/>
					<Searchbar :placeholder="'Namen des Unternehmens eingeben'" :label="'Unternehmen suchen'" @update-search="handleSearchQueryName"/>
			</div>
			<CompanyTable :headers="headers" :rows="filteredCompanies" @update-company="handleChangedCompany" @delete-company="handleDeleteCompany"/>
			<h1 class="section-title">Unternehmen erstellen</h1>
			<CompanyFormCreate v-if="industries.length > 0" @add-company="handleAddCompany" :industries="industries"/>
			<FileUpload
				:dataCategory="'Companies'"
				:uploadSuccess="uploadSuccess"
				:uploadError="uploadError"
				@file-uploaded="handleFileUpload"
			/>
	</div>
</template>
<script setup>
	import axiosInstance from "@/axios";
	import { ref, onMounted, computed } from 'vue';

	import CompanyTable from '../molecules/CompanyTable.vue';
	import CompanyFormCreate from '../molecules/CompanyFormCreate.vue';
	import Searchbar from '../atoms/Searchbar.vue'; 
	import FileUpload from '../molecules/FileUpload.vue';

	//Services importieren
	import { uploadJsonData } from '@/services/uploadService';
	import { validateJson } from '@/services/validateJsonService';

	const companies = ref([]);
	const industries = ref([]);
	const searchQueryName = ref(''); 
	const searchQueryId = ref('');  
	const activeFilter = ref('name');

	// for FileUpload
	const uploadSuccess = ref(false);
	const uploadError = ref("");
		
	const headers = ['ID', 'Name', 'Adresse','','Kontakt/Person','Email/Telefon', 'Longitude/Latitude','']; 

	onMounted(() => {
		getCompanies(); 
		getIndustries();
		window.addEventListener('resize', updateWindowSize); 
	})

	//**************************************/
	//************ Get Companies ***********/
	//**************************************/
	async function getCompanies() {
		try {
				const response = await axiosInstance.get('/company');
				companies.value = response.data;
				console.log(companies.value);
		} catch (error) {
				console.error('Error fetching :', error);
		}
	}

	//**************************************/
	//************* Add Company ************/
	//**************************************/
	async function handleAddCompany(createdCompany, industryId) {
			console.log(createdCompany);
			try {
					const response = await axiosInstance.post(`/company/${industryId}`, createdCompany);
					console.log('Success:', response.data);
					await getCompanies();
			} catch (error) {
					console.error('Error:', error);
			}
	}

	//**************************************/
	//*********** Update Company ***********/
	//**************************************/
	async function handleChangedCompany(editedCompany, companyId){ 
			try {
					console.log(editedCompany); 
					console.log(companyId); 
					const response = await axiosInstance.put(`/company/${companyId}`, editedCompany);
					console.log('Success:', response.data);

					// add edited company to local list after the backend call was successfully 
					const index = companies.value.findIndex((company) => company.id === editedCompany.id);

					if(index !== -1){
							companies.value[index] = { ...companies.value[index], ...editedCompany }; 
					}

			} catch (error) {
					console.error('Error:', error);
			}      
	};

	//**************************************/
	//*********** Delete Company ***********/
	//**************************************/
	async function handleDeleteCompany(companyId) {
			try {
					console.log(companyId);
					const response = await axiosInstance.delete(`/company/${companyId}`);
					companies.value = companies.value.filter(item => item.id !== companyId);
					console.log(response);
			} catch (error) {
					console.error('Error deleting sdgTarget')
			}

	}
	//**************************************/
	//*********** Get Industries ***********/
	//**************************************/
	async function getIndustries() {
		try {
			const response = await axiosInstance.get('/industry');
			industries.value = response.data;
			console.log(industries.value);
		} catch (error) {
			console.error('Error fetching people:', error);
		}
	}
	//**************************************/
	//********** Filter Companies **********/
	//**************************************/
	
	// Computed Property für die gefilterte Listen von Unternehmen 
	const filteredCompanies = computed(() => {
			if (activeFilter.value === 'name' && searchQueryName.value) {
					// Filter nach Namen 
					return companies.value.filter((company) =>
							company.name.toLowerCase().includes(searchQueryName.value.toLowerCase())
					);
			} else if (activeFilter.value === 'id' && searchQueryId.value) {
					// Filter nach Id
					return companies.value.filter((company) => 
							company.id.toString().includes(searchQueryId.value)
					); 
			}
			return companies.value;
	}); 



	//*********** Searchbar name ***********/
	const handleSearchQueryName = (query) => {
			searchQueryName.value = query; 
			activeFilter.value = 'name'; 
	}
	//************ Searchbar id ************/
	const handleSearchQueryId = (query) => {
			searchQueryId.value = query
			activeFilter.value = 'id'; 
	}


	//**************************************/
	//***************breite ****************/
	//**************************************/
	const breite = ref(window.innerWidth);

	function updateWindowSize(){
			breite.value = window.innerWidth; 
	}; 


	/******************************************/
	/************* handle Upload **************/
	/******************************************/

	async function handleFileUpload(jsonData) {
		uploadSuccess.value = false; 
		uploadError.value = ""; //Zurücksetzen der Fehlernachricht

		if (!validateJson(jsonData, "company")){
			console.error("Ungültige JSON-Struktur"); 
			return;
		}

		const response = await uploadJsonData(jsonData, "/company/fileUpload");
		
		if(response.success){
			uploadSuccess.value = true;
		} else {
			uploadError.value = "Fehler beim hochladen der Datei: " + response.error; 
		}
	}

</script>


<style scoped>
	.container-company-settings-view {
			margin: 12px;
			display:flex; 
			flex-direction: column;
			gap: 16px; 
	}
	.searchbars {
			display: flex; 
			flex-direction: row; 
	}
	.section-title {
			font-size: 24px;
			font-weight: bold;
			color: black;
			margin-top: 30px; 
			/* border-bottom: 2px solid #2a9d8f; */

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