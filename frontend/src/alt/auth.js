import axios from '@/axios';
import { ref } from 'vue';



// this is the tokenmanagement if the token is stored in the localstorage
// current status is that the token is stored in http-only cookie
// whitch is the safest way

const isAuthenticated = ref(false);

async function login(username, password) {
    try {
      // axios stellt die Verbindung zum Backend her 
      // post beinhaltet den body der an das backend geschickt wird 
      console.log("try login");
      const response = await axios.post('/account/login', { 
        
          "username": username,
          "passwcheckAuthord": password
        
      });
      console.log(response.data);
      
      if (response.status === 200) {
        // Token wird aus der Antwort extrahiert
        const token = response.data.token; 
        // Speichern des Tokens im localStorage
        localStorage.setItem('authToken', token);
        // Authentifizierungsstatus auf 'true' setzen
        isAuthenticated.value = true;
        return true; 
  
    }
    } catch (error) {
      // Fehlerbehandlung, z.B. Anzeigen einer Fehlermeldung
      console.error('Login error:', error);
      errorMessage.value = 'Fehler beim Einloggen. Bitte überprüfen Sie Ihre Anmeldeinformationen.';
      isAuthenticated.value = false; 
      alert('Login failed. Please try again.');
      return false;
    }
}

function logout() {
  // Logout: Entfernen des Tokens aus dem localStorage und Status auf 'false' setzen
  isAuthenticated.value = false;
  localStorage.removeItem('authToken');
}
  
function checkAuth() {
  // Überprüfen, ob ein Token im localStorage vorhanden ist
  const token = localStorage.getItem('authToken');
  isAuthenticated.value = !!token;
  return isAuthenticated.value;
}

function isTokenExpired() {
  const token = localStorage.getItem('authToken'); 
  if(!token){
    return true;
  }
  const payload = JSON.parse(atob(token.split('.')[1])); // JWT dekodieren
  const expiration = payload.exp * 1000;  // Ablaufzeit in Millisekunden
  return expiration < Date.now();
}

async function refreshToken() {
  const refreshToken = localStorage.getItem('refreshToken');
  try {
      const response = await axios.post('/account/refresh-token', { refreshToken });
      const { token, refreshToken: newRefreshToken } = response.data;
      localStorage.setItem('authToken', token);
      localStorage.setItem('refreshToken', newRefreshToken);
  } catch (error) {
      console.error("Error refreshing token:", error);
      logout(); // Falls das Refresh-Token ungültig ist
  }
}

export { isAuthenticated, login, logout, checkAuth, isTokenExpired, refreshToken };