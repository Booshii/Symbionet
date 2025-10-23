<template>
  <div class="table-container">
    <form @submit.prevent="emitUser">
      
      <div class="form-group">
        <label for="username">Benutzername</label>
        <input type="text" id="username" v-model="createdUser.username" required>
      </div>
      <!-- Name -->
      <div class="form-group">
        <label for="name">Name</label>
        <input type="text" id="name" v-model="createdUser.name" required>
      </div>

      <!-- Unternehmen Dropdown -->
      <div class="form-group">
        <label>Unternehmen auswählen</label>
        <DropDownSelect
          :options="companyDropdownOptions"
          :label="'companyDropdownSelect'"
          :placeholder="'Unternehmen auswählen'"
          @update:selectedItem="handleCompanySelect"
        />
      </div>

      <!-- Telefonnummer -->
      <div class="form-group">
        <label for="phoneNumber">Telefon</label>
        <input type="text" id="phoneNumber" v-model="createdUser.phoneNumber" required>
      </div>

      <!-- E-Mail -->
      <div class="form-group">
        <label for="email">E-Mail</label>
        <input type="email" id="email" v-model="createdUser.email" required>
      </div>
      <div class="form-group">
        <label for="password">Passwort</label>
        <input type="password" id="password" v-model="createdUser.password" required>
        <input type="password" id="password" required :style="{ marginTop: '8px' }">
      </div>

      <!-- Erstellen-Button -->
      <button type="submit" class="submit-button">Erstellen</button>
    </form>
  </div>
</template>

<script setup>
import { ref, defineProps, defineEmits } from 'vue';
import DropDownSelect from '../atoms/DropDownSelect.vue';

const props = defineProps({
  companies: {
    type: Array,
    required: true
  }
});

const companyDropdownOptions = ref([...props.companies]);
console.log(companyDropdownOptions.value); 

const selectedCompany = ref(null);
const createdUser = ref({
  username: '',
  email: '',
  password: '',
  name: '',
  phoneNumber: '',
});


const emit = defineEmits(['add-user']);
function emitUser() {
  emit('add-user', createdUser.value, selectedCompany.value?.id);
}

function handleCompanySelect({ selectedItem }) {
  if (!selectedItem) return;
  selectedCompany.value = selectedItem; 
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
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

/* Form Styling */
form {
  display: flex;
  flex-direction: column;
  gap: 15px;
}

/* Eingabefelder */
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
}

/* Labels */
label {
  font-weight: bold;
  color: black;
  margin-bottom: 5px;
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

/* Button */
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
