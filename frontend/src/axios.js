import axios from 'axios';
import router from '@/router';
import { useAuthStore } from "@/store/authStore"; 


  const axiosInstance = axios.create({
      baseURL: import.meta.env.VITE_API_URL, // Passe die URL deines Backends an
      
      withCredentials: true,
    });

  // function-Map 
  // zentrale Speicherung der gängigen Fehler und ihre Behandlung 
  const errorHandlers = {
    401: () => {
      if(router.currentRoute.value.path == '/login'){
        console.warn('401 auf Login-Seite - keine Umleitung')
      } else {
        console.warn('Token abgelaufen, User wird ausgeloggt...');
        useAuthStore.logout(); 
        router.push('/login'); 
      }
    },
    403: () => alert("keine Berechtigung für diese Aktion"),
    404: () => alert("Seite nicht gefunden"),
    500: () => alert("Serverfehler"),
  };

  axiosInstance.interceptors.response.use(response => response, async (error) => {
    if (error.response) {
      console.error("❌ Fehlerstatus:", error.response.status);
      console.error("❌ Fehlermeldung:", error.response.data);
      console.error("❌ Headers:", error.config.headers);
      console.error("❌ Request-URL:", error.config.url);
  } else {
      console.error("❌ Netzwerkfehler oder Server nicht erreichbar:", error.message);
  }
    if (!error.response) {
      console.error('Network Error:', error); 
      alert("Es besteht ein Problem mit der Verbindung. Bitte überprüfe deine Internetverbindung.");
    } else {
      const handleError = errorHandlers[error.response.status]; 
      if (handleError) {
        handleError();
      } else {
        console.warn(`Unerwarteter API-Fehler: ${error.response.status}`, error.message);
        alert("Ein unbekannter Fehler ist aufgetreten.");
      }
    }
    return Promise.reject(error);
  });

export default axiosInstance;