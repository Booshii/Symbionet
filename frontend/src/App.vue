<script setup>
  import {RouterView} from "vue-router"
  import Nav from "./components/Nav.vue"
  import Footer from "./components/Footer.vue"
  import FooterEmpty from "./components/organisms/FooterEmpty.vue"
  // import Component from "./components/Component.vue"
  
</script>


<template>
  <div class="container">
    <Nav />
    <!-- <Component/>  -->
    <RouterView />
    <FooterEmpty/> 
    <!-- <Footer /> -->
    
  </div>
</template>



<style>


/* Wurzelinstanz von vue - kein standard css tag wie '*' */
html, body, #app{
  margin: 0; 
  min-height: 100vh; 
  position: relative; 
  /* background-color: #f1f1f1; */
}

/* universal tag der sich auf jedes TAG bezieht */
*{
  
  padding: 0; 
  margin: 0; 
  box-sizing: border-box;

  font-family: ui-sans-serif, sans-serif;

}



.container{
  display: flex;
  flex-direction: column;
  align-items: center;
  
}
</style>