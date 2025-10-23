<template>
    <div id="dashboard">
        <!-- <Sidebar :collapsed="isCollapsed" @toggle="toggleSidebar" @selectComponent="changeComponent" />  -->
        <UserSidebar @selectComponent="changeComponent" /> 
        <!-- <div id="content" :class="{ expanded: isCollapsed }"> -->
        <div id="content">
            <!-- <component :is="currentComponent" @contentLoaded="handleResize"></component> -->
            <component :is="currentComponent"></component>
        </div>
    </div>
</template>
<script setup>
    import { reactive, ref, onMounted, onBeforeUnmount, watch } from 'vue';
    
    import UserSidebar from '../components/organisms/UserSidebar.vue'; 

    import CompanySettingsView from '../components/organisms/CompanySettingsView.vue';
    import SdgSettingsView from '../components/organisms/SdgSettingsView.vue';
    import MapSettingsView from '../components/organisms/MapSettingsView.vue';
    import UserAccountSettingsView from '../components/organisms/UserAccountSettingsView.vue';
    import IndustrySettingsView from '../components/organisms/IndustrySettingsView.vue';
    
    const components = {
        CompanySettingsView,
        SdgSettingsView,
        MapSettingsView,
        UserAccountSettingsView,
        IndustrySettingsView
    };
    // const currentComponent = ref('components.UserInformationView');
    const currentComponent = ref(components.UserAccountSettingsView); // Setze die Standardkomponente auf UserInformationView
    
    function changeComponent(component) {
        currentComponent.value = components[component]; 
    }











    

    //******* einklappen der Sidebar ********/

    // const isSidebarCollapsed = ref(false); 

    // //Funktion zum Wechseln der Komponente
    // const changeComponent = (component) => {
    //     currentComponent.value = component; 
    //     handleResize(); 
    // };
    // // Funktion zum ein und ausklappen 
    // const toggleSideBar = () => {
    //     isSidebarCollapsed.value = !isSidebarCollapsed.value; 
    // };
    // //Funktion zu Berechnung des Platzbedarfs und Steuerung der Sidebar
    // const handleResize = () => {
    //     const contentElement = document.getElementById('content'); 
    //     const dashboardWidth = document.getElementById('dashboard'). clientWidth; 
    // };
    
</script>


<style scoped>
    #dashboard{
        display: flex;
        flex-direction: row;
        width: 100vw;
    }
    #content{
        padding-left:20px; 
        flex-grow: 1;
    }
    
</style>