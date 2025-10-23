import { defineStore } from 'pinia'; 
import { ref, computed } from 'vue'; 
import axiosInstance from '@/axios';


export const useAuthStore = defineStore('auth', () => {

  //States
  const user = ref(null);
  const isAuthenticated = ref(false);


  // computed props
  // hier aber redundant 
  // const isLoggedIn = computed(() => isAuthenticated.value); 
  
  // wo kommen die credentials her?
  async function login(credentials) {
    try {
      const response = await axiosInstance.post('/account/login', credentials);
        console.log('Login Response:', response.data);
      isAuthenticated.value = true;
      user.value = response.data;
      // hier doch Token in lokalstorage speichern als schnelle Lösung
      const token = response.data.token;
      if (token) {
        localStorage.setItem('access_token', token);
      } else {
        console.warn('Kein Token im Login-Response gefunden.');
      }

    } catch (error) {
      console.error('Login error', error);
      throw error;
    }
  }

  async function logout() {
    try {
      await axiosInstance.post('/account/logout'); // Cookie wird invalidiert
    } catch (error) {
      console.warn('Logout-Fehler:', error);
    }

  user.value = null;
  isAuthenticated.value = false;
  }

  function setAuth(authStatus, userData) {
    isAuthenticated.value = authStatus;
    user.value = userData;  
  }

  const role = computed(() => user.value?.role || null);
  const companyId = computed(() => user.value?.companyId || null);
  const isAdmin = computed(() => role.value === 'Admin'); 
  


  return { user, isAuthenticated, login, logout, setAuth, role, companyId, isAdmin }; 
});