<template>
	<div class="table-container">
		<form @submit.prevent="emitSdgTarget">
			<!-- Titel -->
			<div class="form-group">
				<label for="title">Titel</label>
				<input type="text" id="title" v-model="createdSdgTarget.title" required>
			</div>

			<!-- SDG Dropdown -->
			<div class="form-group">
				<label>SDG auswählen</label>
				<DropDownSelect
					:options="dropdownOptions"
					:label="'sdgDropdownSelect'"
					:placeholder="'Sdg auswählen'"
					@update:selectedItem="handleSelect"
				/>
			</div>

			<!-- Ausgewählte SDGs -->
			<div class="selectedSdgTypes">
				<ul>
					<li v-for="(item, index) in selectedItems" :key="index">
						{{ item.name }}
						<button class="delete-button" @click="handleDelete(item)">X</button>
					</li>
				</ul>
			</div>

			<!-- Unternehmen suchen -->
			<div v-if="authStore.isAdmin" class="form-group search-container">
				<label>Unternehmen suchen</label>
				<Searchbar 
					:placeholder="'Namen des Unternehmens eingeben'" 
					:label="'Unternehmen suchen'" 
					:inputValue="searchQueryCompany"
					@update-search="handleSearchQueryCompany"
					@focus="showSuggestions = true" 
				/> 
				<div v-if="showSuggestions && filteredCompanies.length" class="suggestions">
					<div v-for="company in filteredCompanies" :key="company" class="suggestion" @click="selectCompany(company)">
						{{ company.name }}
					</div>
				</div>
			</div>

			<!-- Beschreibung -->
			<div class="form-group">
				<label for="description-area">Beschreibung</label>
				<textarea id="description-area" v-model="createdSdgTarget.description" required></textarea>
			</div>

			<!-- Subgoals hinzufügen -->
			<div class="form-group">
				<AddSubGoal ref="addSubGoalRef" />
			</div>

			<!-- Checkbox -->
			<div class="form-group checkbox-container">
				<label>
					<input type="checkbox" v-model="createdSdgTarget.isDone">
					Maßnahme umgesetzt
				</label>
			</div>


		<div class="form-group checkbox-container">
						<label>
									<input type="checkbox" v-model="createdSdgTarget.isDone">
									Veröffentlichen
							</label>
					</div>

			<!-- Button -->
			<button type="submit" class="submit-button">Erstellen</button>
		</form>
	</div>
</template>

<script setup>
import { ref, computed, defineEmits, onMounted } from 'vue'; 
import DropDownSelect from '../atoms/DropDownSelect.vue'; 
import Searchbar from '../atoms/Searchbar.vue';

import { useAuthStore } from '@/store/authStore';

const authStore = useAuthStore();

const props = defineProps({
	companies: { type: Array, required: true },
	sdgTypes: { type:Array, required: false }
});

const companies = ref([...props.companies]);
const selectedCompany = ref(null);
const selectedItems = ref([]); 
const dropdownOptions = ref([...props.sdgTypes]); 
const createdSdgTarget = ref({
	title: '',
	description: '',
	isDone: false,
	sdgTypeIds: [],
	subGoals: [{ title: '', description: '' }]
});

onMounted(() => {
	if (!authStore.isAdmin && authStore.companyId) {
		// set company directly
		const foundCompany = companies.value.find(c => c.id === authStore.companyId);
		if (foundCompany) {
			selectedCompany.value = foundCompany;
			searchQueryCompany.value = foundCompany.name;
		}
	}
})

const emit = defineEmits(['add-sdgTarget']);
function emitSdgTarget() {
	emit('add-sdgTarget', createdSdgTarget.value, selectedCompany.value?.id);
}

/************** Dropdown Handling **************/
function handleSelect({ selectedItem }) {
	if (!selectedItem) return;
	createdSdgTarget.value.sdgTypeIds.push(selectedItem.id);
	dropdownOptions.value = dropdownOptions.value.filter(item => item.id !== selectedItem.id);
	selectedItems.value.push(selectedItem);
	console.log(createdSdgTarget.value.sdgTypeIds)
}

function handleDelete(deletedItem) {
	createdSdgTarget.value.sdgTypeIds = createdSdgTarget.value.sdgTypeIds.filter(id => id !== deletedItem.id);
	dropdownOptions.value.push(deletedItem); 
	selectedItems.value = selectedItems.value.filter(item => item.id !== deletedItem.id);
}

/************** Suchleiste für Unternehmen **************/
const searchQueryCompany = ref(''); 
const activeFilter = ref('name'); 
const showSuggestions = ref(false); 

const filteredCompanies = computed(() => {
	return companies.value.filter(company => 
		company.name.toLowerCase().includes(searchQueryCompany.value.toLowerCase())
	);
}); 

function handleSearchQueryCompany(query) {
	searchQueryCompany.value = query; 
	showSuggestions.value = true; 
}

function selectCompany(company) {
	selectedCompany.value = company; 
	searchQueryCompany.value = company.name; 
	showSuggestions.value = false; 
}
</script>

<style scoped>
/* Container für das Formular */
.table-container {
	display: flex;
	flex-direction: column;
	gap: 10px;
	width: 100%;
	background-color: #ffffff;
	border-radius: 15px;
	border: 1px solid #e1e8ed;
	padding: 20px;
	margin-top: 10px; /* Abstand nach oben hinzugefügt */
	box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

/* Form Styling */
form {
	display: flex;
	flex-direction: column;
	gap: 15px;
}

/* Eingabefelder */
input, textarea, select {
	width: 100%;
	padding: 10px;
	border: 1px solid grey;
	border-radius: 6px; /* Weniger abgerundete Ecken */
	font-size: 14px;
	background-color: white;
	color: black;
	outline: none;
}

/* Kein grüner Rand mehr */
input:focus, textarea:focus, select:focus {
	border-color: grey;
	box-shadow: none;
}

/* Labels */
label {
	font-weight: bold;
	color: black;
	margin-bottom: 5px;
}

/* Ausgewählte SDG-Typen */
.selectedSdgTypes {
	background-color: #f3faff;
	padding: 10px;
	border-radius: 6px;
}

.selectedSdgTypes ul {
	display: flex;
	flex-wrap: wrap;
	gap: 10px;
	list-style-type: none;
	margin: 0;
	padding: 0;
}

.selectedSdgTypes li {
	background-color: #2a9d8f;
	color: white;
	padding: 5px 10px;
	border-radius: 6px;
	display: flex;
	align-items: center;
	gap: 5px;
}

.delete-button {
	background: transparent;
	border: none;
	color: white;
	cursor: pointer;
	font-weight: bold;
}

/* Checkbox */
.checkbox-container {
	display: flex;
	align-items: center;
	gap: 10px;
	color: black;
}

.checkbox-container input {
	width: 18px;
	height: 18px;
	cursor: pointer;
}

/* Suchleiste */
.search-container {
	position: relative;
	width: 100%;
}

/* Vorschläge */
.suggestions {
	position: absolute;
	top: 100%;
	left: 0;
	right: 0;
	border: 1px solid #ddd;
	background: white;
	z-index: 10;
	max-height: 150px;
	overflow-y: auto;
}

.suggestion {
	padding: 8px;
	cursor: pointer;
}

.suggestion:hover {
	background-color: #f0f0f0;
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
