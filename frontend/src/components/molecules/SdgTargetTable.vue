<template>
	<div class="table-container">
		<Popup v-if="showPopup" @confirm="handleConfirmDelete" @cancel="handleCancelDelete">
			Wollen Sie die ausgewählte Maßnahme löschen?
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
				<template v-for="sdgTarget in rows" :key="sdgTarget.id">
					<tr :class="{ leftborder: showEditRowId === sdgTarget.id }">
						<td>
							<span>{{ sdgTarget.id }}</span>
						</td>
						<td>
							<span class="sdgTarget-title">{{ sdgTarget.title }}</span> <br>
							<span>{{ sdgTarget.companyName }}</span>
						</td>
						<td>
							<span>{{ sdgTarget.description }}</span>
						</td>
						<td>
							<input class="checkIsDone" 
								type="checkbox"
								:checked="sdgTarget.isDone"
								:disabled="true" 
							/>
						</td>
						<td>
							<input class="checkIsPublished" 
								type="checkbox"
								:checked="sdgTarget.isPublished"
								:disabled="true" 
							/>
						</td>
						<td>
							<button v-if="showEditRowId === sdgTarget.id" @click="saveChanges(sdgTarget.id)" class="save-button">
								Speichern
							</button>
							<button v-else @click="handleEditSdgTarget(sdgTarget)" class="edit-button">
								Bearbeiten
							</button>
						</td>
					</tr>
					<!-- Zusätzliche Reihe für SDG-Typen -->
					<tr >
						<td colspan="6" >
							<strong>SDG-Typen:</strong>
							<span v-for="sdg in sdgTarget.sdgTypes" :key="sdg.id" class="sdg-type-badge"
									:style="{ backgroundColor: sdg.color }">
								{{ sdg.number }} - {{ sdg.name }}
							</span>
						</td>
					</tr>

				<transition-group name="slide-fade">

						<tr v-if="showEditRowId === sdgTarget.id" id="edit-sdgTarget-row">
							<td></td>
							<td>
									<label for="sdg-target-title">Titel</label> <br>
									<input type="text" id="sdg-target-title" v-model="editSdgTarget.title"> <br>

							</td>
							<td>
									<label for="sdg-target-description">Beschreibung</label> <br>
									<input type="text" id="sdg-target-description" v-model="editSdgTarget.description">
							</td>
							<td>
									<label for="sdg-target-is-done">Umgesetzt</label> <br>
									<input class="checkIsDone" 
											type="checkbox"
											v-model="editSdgTarget.isDone"
											:disabled="false" 
									/>                            
							</td>
							<td>
									<label for="sdg-target-picture">Veröffentlicht</label> <br>
									<input class="checkIsDone" 
											type="checkbox"
											v-model="editSdgTarget.isPublished"
											:disabled="false" 
									/>
							</td>
							<td>
									<button type="default" class="delete-button" @click="handleDeleteRequest(editSdgTarget)">
											Löschen
									</button>
							</td>
						</tr>
						<!-- Editierbare Zeile für SDG-Typen -->
						<tr v-if="showEditRowId === sdgTarget.id">
							<td colspan="6">
								<label>SDG-Typen bearbeiten:</label>
									<Dropdown
										:options="sdgTypesDropdownOptions"
										:label="'sdgTypesDropdownSelect'"
										:placeholder="'sdg auswählen'"
										@update:selected-item="handleSelectDropdown"
										/>
									<span class="sdg-type-edit-badge" v-for="sdg in selectedSdgTypes" :key="sdg.id"
										:style="{ backgroundColor: sdg.color }">
										{{ sdg.number }} - {{ sdg.name }}
										<button class="delete-sdg-type-button" @click="handleSdgTypeDeleteButton(sdg)">X</button>
							</span>
							</td>
						</tr>					

				</transition-group>
			</template>
		</tbody>
		</table>
	</div>
</template>

<script setup>
import { defineProps, ref, defineEmits, reactive, computed } from 'vue'; 
import Popup from '../atoms/Popup.vue';
import Dropdown from '../atoms/DropDownSelect.vue';

const props = defineProps({
    headers: { type: Array, required: true },
    rows: { type: Array, required: true },
		sdgTypes: { type:Array, required: false, default: () => [] },
}); 

const showPopup = ref(false);
const showEditRowId = ref(null);
const editSdgTarget = reactive({
    id: '', title: '', company: '', description: '', isDone: '', isPublished: '', sdgTypeIds: []
}); 
// ein extra Array was die ausgewählten sdgTypes 
// in der editrow in der Tabelle smit number und name speichert
// das editSdgTarget nimmt nur die ids für das Backend

const selectedSdgTypes = computed(() => {
	return editSdgTarget.sdgTypeIds
			.map(id => props.sdgTypes.find(sdg => sdg.id === id))
			.filter(sdg => sdg !==undefined); 
});

const sdgTypesDropdownOptions = ref([...props.sdgTypes]); 

const emit = defineEmits(['save-sdg-target', 'delete-sdg-target']);

function handleEditSdgTarget(sdgTarget) {
    Object.assign(editSdgTarget, {
			...sdgTarget,
			// konvertiert die Sdg-Objekte in eine Liste von ids
			sdgTypeIds: sdgTarget.sdgTypes.map(sdg => sdg.id)
		});
    showEditRowId.value = editSdgTarget.id;
		
}

function saveChanges(sdgTargetId) {
	emit('save-sdg-target', { ...editSdgTarget }, sdgTargetId);
	showEditRowId.value = null;
	editSdgTarget.sdgTypeIds = [];
}

function handleDeleteRequest() {
    showPopup.value = true; 
}

function handleConfirmDelete() {
    emit('delete-sdg-target', editSdgTarget.id);
    showPopup.value = false; 
}

function handleCancelDelete() {
    showPopup.value = false; 
}

/************** Dropdown Handling **************/
function handleSelectDropdown({ selectedItem }) {
	if (!selectedItem) return;

	editSdgTarget.sdgTypeIds.push(selectedItem.id);
	console.log(editSdgTarget); 

	console.log(sdgTypesDropdownOptions.value);
	sdgTypesDropdownOptions.value = sdgTypesDropdownOptions.value.filter(item => item.id !== selectedItem.id);
	console.log(sdgTypesDropdownOptions.value);
}

function handleSdgTypeDeleteButton(deletedItem){
	editSdgTarget.sdgTypeIds = editSdgTarget.sdgTypeIds.filter(id => id !== deletedItem.id);
}
</script>

<style scoped>
.table-container {
    display: flex;
    flex-direction: column;
    gap: 10px;
    width: 100%;
}

/* Tabelle */
table {
    border-collapse: collapse;
    width: 100%;
    font-family: Arial, sans-serif;
    background-color: #f9f9f9;
    border-radius: 20px; /* Abrundung der Ecken */
    border: 1px solid #e0e0e0;
    overflow: hidden;
}

/* Tabellenkopf */
th {
    background-color: white;
    color: black;
    text-align: left;
    border-bottom: 2px solid #003366;
    padding: 10px 20px;
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
    padding: 10px 24px;
    border-bottom: 1px solid #e0e0e0;
    color: #333;
}

/* Buttons */
.edit-button, .save-button, .delete-button {
    border: none;
    padding: 8px 12px;
    cursor: pointer;
    border-radius: 5px;
    transition: background-color 0.3s;
    color: white;
}

.edit-button {
    background-color: #2a9d8f;
}

.edit-button:hover {
    background-color: #21867a;
}

.save-button {
    background-color: #004d99;
}

.save-button:hover {
    background-color: #003366;
}

.delete-button {
    background-color: #d9534f;
}

.delete-button:hover {
    background-color: #c9302c;
}

/* Editierbare Zeilen */
/* #edit-sdgTarget-row {
	border-top: 2px solid #004d99;
	background-color: #f1faff;
	box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.1);
} */

/* Links markierte Zeilen */
.leftborder {
    border-left: 4px solid #2a9d8f;
    background-color: #e6f4f1;
}

/* Input-Felder */
input {
    border: 1px solid #ccc;
    padding: 5px;
    border-radius: 3px;
    width: 95%;
}

/* Animation */
.slide-fade-enter-active,
.slide-fade-leave-active {
    transition: all 1s ease;
}

.slide-fade-enter-from,
.slide-fade-leave-to {
    max-height: 0;
    opacity: 0;
    overflow: hidden;
}

.slide-fade-enter-to,
.slide-fade-leave-from {
    max-height: 500px;
    opacity: 1;
    overflow: hidden;
}
.sdg-type-badge{
	background-color: #f3faff;
	padding: 2px;
	border-radius: 6px;
	margin: 4px; 

}

.sdg-type-edit-badge{
	background-color: #f3faff;
	padding: 2px;
	border-radius: 6px;
	margin: 4px; 

}

.delete-sdg-type-button {
	background: transparent;
	border: none;
	color: white;
	cursor: pointer;
	font-weight: bold;
	
}
</style>
