<template>
    <div class="container">
      <table>
        <thead>
          <tr>
            <th v-for="(head, index) in headers" :key="index">
              {{ head }}
            </th>
          </tr>
        </thead>
        <tbody>
          <template v-for="company in rows" :key="company.id">
            <!-- Row with conditional styling -->
            <tr :class="{ leftborder: showEditCompanyRowId === company.id }">
              <td>
                <span class="company-name">{{ company.name }}</span> <br />
                <span class="company-id">{{ company.id }}</span>
              </td>
              <td colspan="2">
                <span>{{ company.street }} {{ company.streetNumber }} </span>
                <br />
                <span>{{ company.postalcode }} {{ company.city }}</span>
              </td>
              <td>
                <span>{{ company.contactPerson }}</span>
              </td>
              <td>
                <span>{{ company.contactEmail }} </span>
                <br />
                <span>{{ company.contactTel }} </span>
              </td>
              <td>
                <span>{{ company.longitude }} </span>
                <br />
                <span>{{ company.latitude }} </span>
              </td>
              <td>
                <button @click="showEditRow(company)" class="edit-button">
                  Bearbeiten
                </button>
              </td>
            </tr>
            <transition name="slide-fade">
              <tr v-if="showEditCompanyRowId === company.id" id="edit-company-row">
                <td>
                  <label for="company-name">Unternehmensname</label> <br />
                  <input type="text" id="company-name" v-model="editCompany.name" />
                  <br />
                  <label for="company-id">Unternehmens-Id</label> <br />
                  <input id="company-id" type="text" v-model="editCompany.id" />
                </td>
                <td>
                  <label for="company-street">Straße</label> <br />
                  <input type="text" id="company-street" v-model="editCompany.street" />
                  <br />
                  <label for="company-city">City</label> <br />
                  <input type="text" id="company-city" v-model="editCompany.city" />
                </td>
                <td>
                  <label for="CompanyStreetNumber">Hausnummer</label> <br />
                  <input id="CompanyStreetNumber" type="text" class="short-input" v-model="editCompany.streetNumber" />
                  <br />
                  <label for="CompanyPostal">PLZ</label> <br />
                  <input id="CompanyPostal" type="text" class="short-input" v-model="editCompany.postalcode" />
                  <br />
                </td>
                <td>
                  <label for="CompanyContactPerson">Kontaktperson</label> <br />
                  <input id="CompanyContactPerson" type="text" v-model="editCompany.contactPerson" />
                  <br />
                </td>
                <td>
                  <label for="CompanyContactEmail">Email</label> <br />
                  <input id="CompanyContactEmail" type="text" v-model="editCompany.contactEmail" />
                  <br />
                  <label for="CompanyTel">Tel</label> <br />
                  <input id="CompanyTel" type="text" v-model="editCompany.contactTel" />
                </td>
                <td>
                  <label for="CompanyLongitude">Longitude</label> <br />
                  <input id="CompanyLongitude" type="text" v-model="editCompany.longitude" />
                  <br />
                  <label for="CompanyLatitude">Latitude</label> <br />
                  <input id="CompanyLatitude" type="text" v-model="editCompany.latitude" />
                </td>
                <td>
                  <button @click="saveChanges(company.id)" class="save-button">
                    Speichern
                  </button>
                </td>
              </tr>
            </transition>
          </template>
        </tbody>
      </table>
    </div>
  </template>
  
  <script setup>
  import { defineProps, ref, defineEmits, reactive } from 'vue';
  
  const props = defineProps({
    headers: {
      type: Array,
      required: true,
    },
    rows: {
      type: Array,
      required: false,
    },
    modelValue: Array,
  });
  
  const showEditCompanyRowId = ref(false);
  
  const editCompany = reactive({
    name: '',
    street: '',
    streetNumber: '',
    postalcode: '',
    city: '',
    contactPerson: '',
    contactEmail: '',
    contactTel: '',
    latitude: '',
    longitude: '',
  });
  
  function showEditRow(company) {
    showEditCompanyRowId.value = company.id;
    Object.assign(editCompany, company);
  }
  
  const emit = defineEmits(['update:modelValue']);
  
  function saveChanges(companyId) {
    emit('save-company', { ...editCompany }, companyId);
    showEditCompanyRowId.value = null;
  }
  </script>
  
  <style scoped>
  .container {
    display: flex;
  }
  
  table {
    border-collapse: collapse;
    width: 100%;
    font-family: Arial, sans-serif;
    background-color: #f9f9f9;
    border: 1px solid #e0e0e0;
  }
  
  th {
    background-color: #004d99;
    color: white;
    padding: 10px;
    text-align: left;
    border-bottom: 2px solid #003366;
  }
  
  td {
    padding: 10px;
    border-bottom: 1px solid #e0e0e0;
    color: #333;
  }
  
  .company-name {
    font-weight: bold;
    color: #004d99;
  }
  
  .edit-button {
    background-color: #2a9d8f;
    color: white;
    border: none;
    padding: 5px 10px;
    cursor: pointer;
    border-radius: 5px;
    transition: background-color 0.3s;
  }
  
  .edit-button:hover {
    background-color: #21867a;
  }
  
  .save-button {
    background-color: #004d99;
    color: white;
    border: none;
    padding: 5px 10px;
    cursor: pointer;
    border-radius: 5px;
    transition: background-color 0.3s;
  }
  
  .save-button:hover {
    background-color: #003366;
  }
  
  .leftborder {
    border-left: 4px solid #2a9d8f;
    background-color: #e6f4f1;
  }
  
  .label {
    color: #555;
  }
  
  input {
    border: 1px solid #ccc;
    padding: 5px;
    border-radius: 3px;
    width: 95%;
  }
  
  .short-input {
    max-width: 100px;
  }
  
  #edit-company-row {
    border-top: 2px solid #004d99;
    border-left: 4px solid #2a9d8f;
    background-color: #f1faff;
    box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.1);
  }
  
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
  