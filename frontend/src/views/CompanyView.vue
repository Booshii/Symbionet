<template>
    <div v-if="SdgTargets && sdgTypes && company" class="company-view-container">
        <div class="company-informations">
            <div class="company-logo">
                <img src="@/assets/Berry.jpg" alt="">
            </div>
            <h1>{{ company.name }}</h1>
            <div class="company-portfolio">
                <p>Branche:<br>
                    {{ company.industryName }} </p>
                <p>Webseite: <br>
                    {{company.websiteLink}} </p>
                <p>Firmenadresse: <br>
                    <span> {{  company.street }} {{ company.streetNumber }}</span>
                    <br>
                    <span> {{ company.postalcode }} {{ company.city }}</span>
                </p>
            </div>
            <div class="company-contact">
                <h2>Kontakt</h2>
                <p>Name: {{ company.contactPerson }} <br>
                    Telefon:   {{ company.contactTel }} <br>
                    E-Mail: {{ company.contactEmail }}</p>
            </div>
            <div class="company-description">
                <h3>Beschreibung</h3>
                <p> Herstellung von: Vliesstoffen (Polyethylen und Polypropylen) und Laminaten für technische Anwendungen, sowie für die Bereiche Medical und Filtration</p>
            </div>
        </div>

        <!-- hier müsste was falsch sein -->
        <CompanySdgTargetsList 

            :sdgTargets="SdgTargets" 
            :sdgTypes="sdgTypes"
        />

        


    </div>
    <p v-else> Lade Unternehmensdaten </p>
</template>

<script setup>
    import CompanySdgTargetsList from '@/components/molecules/CompanySdgTargetsList.vue';
    import axiosInstance from "@/axios";
    import { ref, onMounted } from 'vue';
    import { useRoute } from 'vue-router';

    
    const route = useRoute();
    const companyId = ref(); 
    const company = ref();
    const sdgTypes = ref([]);
    const SdgTargets = ref([]);

    onMounted(() => {
        companyId.value = route.params.id; 
        getCompany(companyId.value);  
        getSdgTypes(); 


    });

    //**************************************/
    //************* Get Company ************/
    //**************************************/
    async function getCompany(id){
        try{
            const response = await axiosInstance.get(`/company/${id}`); 
            if (response.data) {
                company.value = response.data;
                SdgTargets.value = response.data.sdgTargets ?? []; // Falls `sdgTargets` nicht existiert, setze es auf []
            }
                console.log('Success', response.data); 
        } catch (error) {
            console.error('Error fetching people', error); 
        }

    }



    // old try with pregrouped sdgTargets 
    // maybe  later

    // async function getCompany(id){
    //     try{
    //         const response = await axios.get(`http://localhost:5266/api/company/groupedTargets/${id}`); 
    //         company.value = response.data;
    //         console.log('Success', response.data);
    //         groupedSdgTargets.value = company.value.groupedSdgTargetsByColor; 
    //     } catch (error) {
    //         console.error('Error fetching people', error); 
    //     }

    // }

    //**************************************/
    //************ Get SdgTypes ************/
    //**************************************/
    async function getSdgTypes(){
        try{
            const response = await axiosInstance.get(`/sdgtype`); 
            sdgTypes.value = response.data; 
            console.log(sdgTypes.value[1]);
        } catch (error) {
            console.error('Error fetching people', error); 
        }
    }


</script>

<style scoped>
    .company-view-container{
        display: flex;
        flex-direction: column; 
        /* justify-content: center; */
        align-items: center;
        
    }
    .company-informations{
        display: grid;
        grid-template-columns: 1fr 1fr;
        grid-template-rows: repeat(3, 1fr);
        gap: 10px; 

        width: 1200px; 
        height: 560px;;
    }
    .company-logo{
        

        grid-column: 1 / 2; /* Erste Spalte */
        grid-row: 1 / 3; /* Über beide Zeilen */

        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
    }
    .company-portfolio{
        

        grid-column: 2 / 3; /* Zweite Spalte */
        grid-row: 2 / 3; /* Erste Zeile */

        display: flex;
        flex-direction: column;
        align-items: flex-start;
        justify-content: center;
    }
    .company-contact{
        

        grid-column: 3 / 4; /* Zweite Spalte */
        grid-row: 2 / 3; /* Zweite Zeile */

        display: flex;
        flex-direction: column;
        align-items: flex-start;
        justify-content: center;
        
    }
    .company-description{
        
        grid-column: 1 / 4;
        grid-row: 3 / 4; /* Dritte Reihe */
        
    }
    h1 {
        grid-column: 2 / 4;
        grid-row: 1 / 2;
    }
    /* .company-sdg-target-list{
        display: flex;
        flex-direction: column;
        gap: 12px;
    }
     */
</style>