<script setup>
  import { useAuthStore } from "@/store/authStore";
  import { ref } from 'vue';
  import { useRouter } from 'vue-router';

  const username = ref('');
  const password = ref(''); 
  const errorMessage =  ref('');
  const authStore = useAuthStore(); 
  const router = useRouter();
    

  async function handleLogin() {
    try{
      await authStore.login({username: username.value, password: password.value});
      router.push('/admin-dashboard');
    } catch (error) {
      errorMessage.value = 'Fehler beim Einloggen. Bitte überprüfen Sie Ihre Anmeldeinformationen View.';
      console.error('Login error:', error);
    }
  }
  </script>

<template>
    <div class = "login-form-container">
      <h2>Login</h2>
      <form @submit.prevent="handleLogin">
        <div class="username-container">
          <label for="username">Benutzername:</label>
          <input type="text" id="username" v-model="username" required>
        </div>
        <div class="password-container"> 
          <label for="password">Passwort:</label>
          <input type="password" id="password" v-model="password" required>
        </div>
        <button type="submit">Einloggen</button>
      </form>
      <p v-if="errorMessage" style="color: red;">{{ errorMessage }}</p>
    </div>
  </template>

  <style scoped>  
  .login-form-container{
    display: flex;
    flex-direction: column; 
    justify-content: center;
    align-items: center;

    min-height: 480px;
  }
  form {
    margin-top: 20px;
    display: flex;
    flex-direction: column;
    min-width: 1040px;
  }
  .username-container{
    display: flex;
    flex-direction: column;
  }
  .password-container{
    display: flex;
    flex-direction: column;
  }
  
  
  label {
    margin-bottom: 5px;
  }
  
  input {
    padding: 8px;
    margin-bottom: 10px;

  }
  
  button {
    padding: 10px;
    background-color: #00123a;
    color: #fff;
    border: none;
    cursor: pointer;
  }
  
  button:hover {
    background-color: #0056b3;
  }
  
  p {
    margin-top: 10px;
  }
  </style>
  