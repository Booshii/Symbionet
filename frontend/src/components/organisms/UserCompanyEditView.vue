<template>
  <div class="table-container">
    <!-- Benutzerdaten -->
    <form @submit.prevent="saveUserData">
      <h2 class="section-title">Benutzerdaten</h2>

      <div class="form-group">
        <label for="username">Benutzername</label>
        <input id="username" v-model="editUser.username" type="text" required />
      </div>

      <div class="form-group">
        <label for="name">Name</label>
        <input id="name" v-model="editUser.name" type="text" required />
      </div>

      <div class="form-group">
        <label for="email">E-Mail</label>
        <input id="email" v-model="editUser.email" type="email" required />
      </div>

      <div class="form-group">
        <label for="phone">Telefonnummer</label>
        <input id="phone" v-model="editUser.phoneNumber" type="tel" />
      </div>

      <button class="submit-button" type="submit">Benutzerdaten speichern</button>
    </form>

    <!-- Unternehmensdaten -->
    <form @submit.prevent="saveCompanyData">
      <h2 class="section-title">Kontaktdaten des Unternehmens</h2>

      <div class="form-group">
        <label for="contactPerson">Ansprechpartner</label>
        <input id="contactPerson" v-model="editCompany.contactPerson" type="text" />
      </div>

      <div class="form-group">
        <label for="contactEmail">Kontakt-E-Mail</label>
        <input id="contactEmail" v-model="editCompany.contactEmail" type="email" />
      </div>

      <div class="form-group">
        <label for="contactTel">Kontakt-Telefon</label>
        <input id="contactTel" v-model="editCompany.contactTel" type="tel" />
      </div>

      <button class="submit-button" type="submit">Firmendaten speichern</button>
    </form>
  </div>
</template>

<script setup>
import { reactive, onMounted } from 'vue';
import { useAuthStore } from '@/store/authStore';
import axiosInstance from '@/axios';

const authStore = useAuthStore();

const additionalUserData = reactive({
  userId: "",
  username: "",
  userRole: 1,
  name: "",
  email: "",
  address: "",
  phoneNumber: ""
})

const editUser = reactive({
  username: '',
  name: '',
  email: '',
  phoneNumber: ''
});

const editCompany = reactive({
  name: '',
  street: '',
  streetNumber: '',
  postalcode: '',
  city: '',
  industryId: 1,
  contactPerson: '',
  contactEmail: '',
  contactTel: '',
  latitude: '',
  longitude: '',
  websiteLink: ''
});

onMounted(async () => {
  await getotherUserData();

  if (authStore.user) {
    Object.assign(editUser, {
      username: authStore.user.username,
      name: additionalUserData.name,
      email: authStore.user.email,
      phoneNumber: additionalUserData.phoneNumber
    });

    try {
      const res = await axiosInstance.get(`/company/${authStore.companyId}`);
      Object.assign(editCompany, res.data); // vollständig befüllen
      console.log(editCompany);
    } catch (err) {
      console.error('Fehler beim Laden der Firmendaten:', err);
    }
  }
});

async function saveUserData() {
  try {
    await axiosInstance.put(`/account/${authStore.user.id}`, editUser);
    alert('Benutzerdaten erfolgreich gespeichert.');
  } catch (err) {
    console.error('Fehler beim Speichern der Benutzerdaten:', err);
    alert('Fehler beim Speichern der Benutzerdaten.');
  }
}

async function saveCompanyData() {
  try {
    await axiosInstance.put(`/company/${authStore.companyId}`, { ...editCompany });
    alert('Firmendaten erfolgreich gespeichert.');
  } catch (err) {
    console.error('Fehler beim Speichern der Firmendaten:', err);
    alert('Fehler beim Speichern der Firmendaten.');
  }
}

async function getotherUserData() {
    try {
    const response = await axiosInstance.get(`/account/user-client/${authStore.user.id}`, editUser);
    Object.assign(additionalUserData, response.data);
    console.log("past");
    console.log(additionalUserData);
  } catch (err) {
    console.error('Fehler beim Laden der Benutzerdaten:', err);
    alert('Fehler beim Laden der Benutzerdaten.');
  }
}
</script>

<style scoped>
.table-container {
  display: flex;
  flex-direction: column;
  gap: 30px;
  width: 100%;
  background-color: #ffffff;
  border-radius: 15px;
  border: 1px solid #e1e8ed;
  padding: 20px;
  margin-top: 10px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

form {
  display: flex;
  flex-direction: column;
  gap: 15px;
}

input {
  width: 100%;
  padding: 10px;
  border: 1px solid grey;
  border-radius: 6px;
  font-size: 14px;
  background-color: white;
  color: black;
  outline: none;
}

input:focus {
  border-color: grey;
  box-shadow: none;
}

label {
  font-weight: bold;
  color: black;
  margin-bottom: 5px;
}

.section-title {
  margin-top: 10px;
  font-size: 18px;
  font-weight: bold;
  color: #003366;
  border-bottom: 1px solid #ccc;
  padding-bottom: 4px;
}

.submit-button {
  background-color: #2a9d8f;
  color: white;
  border: none;
  padding: 10px;
  border-radius: 6px;
  cursor: pointer;
  font-size: 16px;
  margin-top: 10px;
}

.submit-button:hover {
  background-color: #003366;
}
</style>
