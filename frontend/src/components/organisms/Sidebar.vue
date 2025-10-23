<!-- 
  * Filename: Sidebar.vue 
  * Description: this component provides a sidebar wirh navigation items
  * it allows the selection of various views and submenus 
  * it sends the selected navigation item to  the AdminDashboardView which changes the contentComponent

-->

<script setup>
  import { defineEmits, ref, onMounted, onUpdated } from 'vue'; 
  import { useAuthStore } from '@/store/authStore';

  const authStore = useAuthStore();

  // Change the content-component on the Dashboard
  const emit = defineEmits(['selectComponent']);

  const selectedComponent = ref(null); 

  const adminMenuItems = [
    { label: 'Benutzerverwaltung', component: 'UserAccountSettingsView', icon: new URL('../../assets/svg-emoticons/users_black.svg', import.meta.url).href}, 
    { label: 'SDG Maßnahmen', component: 'SdgSettingsView', icon: new URL('../../assets/svg-emoticons/sdg.svg', import.meta.url).href, subMenu: [{ label: 'Sub 1', name: 'Sub1'}]},
    { label: 'Unternehmensverwaltung', component: 'CompanySettingsView',icon: new URL('../../assets/svg-emoticons/city.svg', import.meta.url).href },
    { label: 'Branchenverwaltung', component: 'IndustrySettingsView', icon: new URL('../../assets/svg-emoticons/list.svg', import.meta.url).href},
    // { label: 'Karteneinstellungen', component: 'MapSettingsView', icon: new URL('../../assets/svg-emoticons/map.svg', import.meta.url).href },

  ];

  // User-spezifische Menüeinträge
  const userMenuItems = [
    { label: 'Meine Daten', component: 'UserCompanyEditView', icon: new URL('../../assets/svg-emoticons/users_black.svg', import.meta.url).href },
    { label: 'SDG Maßnahmen', component: 'SdgSettingsView', icon: new URL('../../assets/svg-emoticons/sdg.svg', import.meta.url).href },
  ];

  const menuItems = authStore.isAdmin ? adminMenuItems : userMenuItems;

  function selectComponent(component) {
    selectedComponent.value = component; 
    emit('selectComponent', component); 
  }
</script>

<template>
  <div class="sidebar" ref="containerSidebar">
    <h1 id="menu">Menu</h1>
    <ul>
      <li
        v-for="menu in menuItems"
        :key="menu.component"
        @click="selectComponent(menu.component)">
        
        <div class="menu-item-first-level" :class="{ active: selectedComponent === menu.component }">
          <img class="pictogram" :src="menu.icon" :alt="'Personenanzahl'">
          {{ menu.label }}
        </div>
        <!-- <ul v-if="selectedComponent === menu.component && menu.subMenu">
          <li v-for="subItem in menu.subMenu" :key="subItem.name">
            &vert; 
            {{ subItem.label }}
          </li>
        </ul> -->
      </li>
    </ul>
  </div>
</template>

<style scoped>
#menu {
  padding: 10px; 
}
.sidebar {
  display: flex;
  flex-direction: column;
  align-items: center;
  min-width: 250px;
  height: 1500px;
  border-right: 0.5px solid gray; 
  border-radius: 10px; 
  margin: 4px; 
  gap: 20px; 

  background-color: #00142B;
  color: white; 
  
  padding: 12px; 
}

.sidebar ul {
  list-style-type: none;
  padding: 0;
  margin: 0;
  display: flex; 
  flex-direction: column;
  gap: 10px; 
  
}


.sidebar li {
  display: block;
  
  /* padding: 10px 20px;  */
  transition: background-color 0.3s, color 0.3s;
  cursor: pointer;
}


.sidebar li div {
  padding: 10px 20px;
}

/* .sidebar li:hover, .sidebar li.active {
  background-color: #68829e40;

} */
/* .sidebar li.active {
  border-top: 0.5px solid gray; 
  border-bottom: 0.5px solid gray; 
} */
.menu-item-first-level{
  display: flex;
  gap: 14px;
}
.menu-item-first-level.active, .menu-item-first-level:hover{
  background-color: #68829e40;
}


.sidebar ul ul {
  list-style-type: none;
  padding-left: 20px;
  margin: 0;
}

.sidebar ul ul li {
  padding: 5px 20px;  /* Ensure padding matches parent li */
  background-color: transparent;  /* Ensure sub-menu items do not get highlighted */
  color: inherit; /* Inherit text color */
}


.sidebar ul ul li:hover {
  display: flex;
  justify-content: space-between;
  
  background-color: #777; /* Slightly different hover color for sub-menu items */
  color: #fff;
}

.pictogram{
  width: 20px;
  height: 20px; 
  filter: invert(1); 
}
@media screen and (max-width: 1600px) {
        
      }

</style>