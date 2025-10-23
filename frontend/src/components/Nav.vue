<script setup>
    import {RouterLink, useRouter} from "vue-router"
    import {ref} from 'vue'
    import { useAuthStore } from '@/store/authStore'

    const auth = useAuthStore();
    const router = useRouter(); 

    const mobile = ref(false);
    const scrollPosition = null;     
    const mobileNav = null;

    function handleLogout() {
      auth.logout();
      localStorage.removeItem('access_token');
      router.push({ name: 'home' }); 
    }
    
    
</script>
<template>

  <header :class="{ 'scrolled-nav': scrollPosition}">
    <nav>
      <!-- <div class="brandings"> -->
        <!-- <img id="NavbarLogo" src="@/assets/Logo.svg" alt="NavbarLogo" > -->
        <div class="logo">
          <p> Maive </p>
        </div>
        
      <div class="links">
        <ul v-show="!mobile" class="navigation">
          <li><RouterLink active-class="active" :to="{name: 'home' }" class="link">Home</RouterLink></li>
          <li><RouterLink active-class="active" :to="{name: 'SDGs' }" class="link">SDGs</RouterLink></li>
          <li><RouterLink active-class="active" :to="{name: 'CompaniesOverview' }" class="link">Unternehmen</RouterLink></li>
          <!-- <li><RouterLink active-class="active" :to="{name: 'GrünesKraftwerk' }" class="link">Grünes Kraftwerk</RouterLink></li> -->
          <li><RouterLink active-class="active" :to="{name: 'MaterialDatenbank' }" class="link">Material Datenbank</RouterLink></li>
          <li><RouterLink active-class="active" :to="{name: 'MaterialMarktplatz' }" class="link">Material Marktplatz</RouterLink></li>
        </ul>
      </div>
      
        <div class="login-button-container">
          <!-- <RouterLink active-class="active" :to="{name: 'login'}" class="login-button">Login</RouterLink> -->
          <template v-if="auth.isAuthenticated">
            <RouterLink :to="{ name: 'AdminDashboard' }" class="username-link">
              {{ auth.user?.username || 'User' }}
            </RouterLink>
            <button class="login-button" @click="handleLogout">Logout</button>
          </template>
          <template v-else>
            <RouterLink :to="{ name: 'login' }" class="login-button">Login</RouterLink>
          </template>        
        </div>
        
        <!-- gerade nicht zu sehen  -->
        
        
        <div class="icon">
          <!-- <i @click="toggleMobileNav" v-show="mobile" class="far fa-bars" :class="{'icon-active' : mobileNav }"></i> -->
          <img src="@/assets/burger-menu.svg" alt="BurgerMenu" @click="toggleMobileNav" v-show="mobile" :class="{'icon-active' : mobileNav }">
        </div>

        <transition name="mobile-nav">
          <ul v-show="mobileNav" class="dropdown-nav">
            <li><RouterLink active-class="active" :to="{name: 'home' }" class="link">Home</RouterLink></li>
            <li><RouterLink active-class="active" :to="{name: '' }" class="link">SDGs</RouterLink></li>
            <li><RouterLink active-class="active" :to="{name: '' }" class="link">Firmen</RouterLink></li>
            <!-- <li><RouterLink active-class="active" :to="{name: '' }" class="link">Grünes Kraftwerk</RouterLink></li> -->
            <li><RouterLink active-class="active" :to="{name: '' }" class="link">Material Datenbank</RouterLink></li>
            <li><RouterLink active-class="active" :to="{name: '' }" class="link">Placeholder</RouterLink></li>
          </ul>
        </transition>

      <!-- </div> -->
    </nav>
  </header>

    
</template>
  


  
  <style scoped>
    header{
      display: flex;
      justify-content: center;  
      width: 100%;
    }
    nav{
      display: flex; 
      align-items: center;
      justify-content: center;
      
      width: 100%; 
      background-color: rgb(15, 23, 42);
      height: 84px;
      margin-bottom: 0px;
    }
    .logo {
      flex: 1; 
      display: flex;
      align-items: center;
      justify-content: center;
      font-size: larger;
      font-weight: bolder;
      color: white;
      font-size: 36px; 
      
    }
    .links{
      flex: 2; 
      font-family: ui-sans-serif, sans-serif;
    }
    
    #NavbarLogo{
    width: 120px;
    height: 60px;

    }
    .navigation{
      display: flex; 
      align-items: center;
      justify-content: space-around;
    /* need a min-width to garantie a space around the itmes*/
      min-width: 760px;
    }
    .link{
      font-family: ui-sans-serif, sans-serif;
      /* font-weight: bold;  */
      color: white;
      list-style: none; 
      text-decoration: none;
      /* to delete the space on the rigth site from a list */
      padding: 0;
      font-size: 18px; 
    }
    /* Notation bei der alle aufgezählten gemeint sind */ 
    /* Wert aus .link überschreiben */
    ul .link{
      color:white;
      
    }
    .login-button-container{
      flex: 1; 
      display: flex;
      justify-content: center;
      align-items: center

    }
    .login-button{
      display: flex;
      align-items: center;
      justify-content: center;
      color: white;  
      border-radius: 4px;
      background-color: orangered; 
      text-decoration: none; /* Entfernt die Unterstreichung */
      text-align: center;
      font-weight: bolder;
      height: 28px;
      width: 96px; 
      
    }
    ul{
        display: flex;
        justify-content: flex-start;
        list-style: none; 
        margin: 0;
        
    }

    .username-link {
      color: white;
      margin-right: 12px;
      font-weight: bold;
      text-decoration: none;
    }
    .login-button {
      cursor: pointer;
    }
  </style>
