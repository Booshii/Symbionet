<template>
	<div class="container-user-settings-view">
			<h2>Benutzerinformationen</h2>
			<Searchbar/>
			<UserTable v-if="users" :rows="users" :headers="headers" @update-user="handleUpdateUser" @delete-user="handleDeleteUser"/>
			<UserFormCreate v-if="companies" :companies="companies" @add-user="handleAddUser"/>

	</div>
</template>

<script setup>
	import axiosInstance from "@/axios";
	import { ref, onMounted} from 'vue'; 
	
	import Searchbar from '../atoms/Searchbar.vue';
	import UserTable from '../molecules/UserTable.vue';
	import UserFormCreate from '../molecules/UserFormCreate.vue';

	const companies = ref(null);
	const users = ref([]); 

	const headers = ["ID", "Benutzername", "Unternehmen", "E-Mail", "Name", "Telefon",""];

	onMounted(() => {
		getCompanies(); 
		getUsers();
	})

	


	//**************************************/
	//************** Get Users *************/
	//**************************************/
	async function getUsers(){
		try {
			const response = await axiosInstance.get('/account/all-users');
			users.value = response.data; 
		} catch (error) {
			console.error('Error fetching people', error)
		}
	}

	//**************************************/
	//************* Add User ***************/
	//**************************************/
	
	async function handleAddUser(createdUser, companyId) {
		try {
			const response = await axiosInstance.post(`/account/register/${companyId}`, createdUser);
			await getUsers();
		} catch (error){
			console.error('General Error:', error.message);
		}
	}

	//**************************************/
	//*********** Update User **************/
	//**************************************/

	async function handleUpdateUser(updatedUser, userId) {
		try {
			const response = await axiosInstance.put(`/account/${userId}`, updatedUser);
			
			// Lokale Userliste aktualisieren
			const index = users.value.findIndex(u => u.id === userId);
			if (index !== -1) {
				users.value[index] = response.data;
			}
		} catch (error) {
			console.error('Fehler beim Aktualisieren des Benutzers:', error);
		}
	}

	//**************************************/
	//************ Get Companies ***********/
	//**************************************/
	async function getCompanies() {
		try {
				const response = await axiosInstance.get('/company');
				companies.value = response.data;
				console.log(companies.value);
		} catch (error) {
				console.error('Error fetching people:', error);
		}
	}

</script>

<style scoped>

</style>