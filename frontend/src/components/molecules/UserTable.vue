<template>
  <div class="table-container">
    <Popup v-if="showPopup" @confirm="handleConfirmDelete" @cancel="handleCancelDelete">
      Wollen Sie den ausgewählten Benutzer löschen?
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
        <template v-for="user in rows" :key="user.id">
          <tr :class="{ leftborder: showEditUserRowId === user.id }">
            <td class="user-id">{{ user.id }}</td>
            <td><span class="user-name">{{ user.username }}</span></td>
            <td><span>{{ user.companyName }}</span></td>
            <td><span>{{ user.email }}</span></td>
            <td><span>{{ user.name }}</span></td>
            <td><span>{{ user.phoneNumber }}</span></td>
            <td>
              <button v-if="showEditUserRowId === user.id" @click="saveChanges(user.id)" class="save-button">Speichern</button>
              <button v-else @click="handleEditUser(user)" class="edit-button">Bearbeiten</button>
            </td>
          </tr>

          <transition name="slide-fade">
            <tr v-if="showEditUserRowId === user.id" id="edit-user-row">
              <td>
                {{ user.id}}
              </td>
              <td>
                <label for="user-name">Benutzername</label><br>
                <input type="text" id="user-name" v-model="editUser.username">
              </td>
              <td>
                <label for="user-company">Unternehmen</label> <br>
                <span>{{ user.companyName }}</span>
              </td>
              <td>
                <label for="user-email">Email</label> <br>
                <input type="text" id="user-email" v-model="editUser.email">
              </td>
              <td>
                <label for="name">Name</label><br>
                <input type="text" id="name" v-model="editUser.name">
              </td>
              <td>
                <label for="phonenumber">Telefon</label><br>
                <input type="text" id="phonenumber" v-model="editUser.phoneNumber">
              </td>
              <td>
                <button class="delete-button" @click="handleDeleteRequest(user.id)">Löschen</button>
              </td>
            </tr>
          </transition>
        </template>
      </tbody>
    </table>
  </div>
</template>

<script setup>
import { ref, defineProps, reactive, defineEmits } from 'vue';
import Popup from '../atoms/Popup.vue';

// Props
const props = defineProps({
  rows: {
    type: Array,
    required: false,
  },
  headers: {
    type: Array,
    required: false,
  }
});

const showPopup = ref(false);
const showEditUserRowId = ref(null);
const userToDeleteId = ref(null);

const editUser = reactive({
  username: '',
  name: '',
  email: '',
  phoneNumber: '',
});

const emit = defineEmits(['update-user', 'delete-user']);

// Funktionen
function handleEditUser(user) {
  Object.assign(editUser, user);
  showEditUserRowId.value = user.id;
}

function saveChanges(userId) {
  emit('update-user', { ...editUser }, userId);
  showEditUserRowId.value = null;
}

function handleDeleteRequest(userId) {
  userToDeleteId.value = userId;
  showPopup.value = true;
}

function handleConfirmDelete() {
  emit('delete-user', userToDeleteId.value);
  showPopup.value = false;
}

function handleCancelDelete() {
  showPopup.value = false;
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
  border-radius: 20px; /* Ecken abgerundet */
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

</style>
