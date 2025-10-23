<template>
    <div class="container-industry-settings-view">
        <h2></h2>
        <Searchbar :placeholder="'Branchenname eingeben'" :label="'Branche suchen'" @update-search="handleSearchQueryName"/>
        <Searchbar :placeholder="'ID der Branche eingeben'" :label="'Branche suchen'" @update-search="handleSearchQueryId"/>
        <IndustryTable :headers="headers" :rows="filteredIndustries" @update-industry="handleChangedIndustry" @delete-industry="handleDeleteIndustry"/>
        <h2 class="section-title">Branche erstellen</h2>
        <IndustryFormCreate @add-industry="handleAddIndustry"/>
        <h2 class="section-title" >Datei hochladen</h2>
        <Upload />

    </div>
</template>

<script setup>
    import axiosInstance from "@/axios";
    import { ref, onMounted, onUpdated, computed } from 'vue';

    import IndustryFormCreate from '../molecules/IndustryFormCreate.vue';
    import IndustryTable from '../molecules/IndustryTable.vue';
    import Searchbar from '../atoms/Searchbar.vue';
    import Upload from '../molecules/CompanyFileUpload.vue';


    //Props

    // reactice references 
    const industries = ref([]);
    const searchQueryName = ref(''); 
    const searchQueryId = ref('');
    const activeFilter = ref('name');
    const headers = ['Name/ID', 'Adresse1', 'Adresse2','Kontakt1','Kontakt2', 'Info','button' ]; 

    onMounted(() => {
        getIndustries(); 
    })

    //**************************************/
    //*********** Get Industries ***********/
    //**************************************/
    async function getIndustries() {
        try {
            const response = await axiosInstance.get('/industry'); 
            industries.value = response.data; 
            console.log(industries.value); 
        } catch (error) {
            console.error('Error fetching people: ', error); 
        }
    }
    //**************************************/
    //********** Add Industries ************/
    //**************************************/
    
    async function handleAddIndustry(createdIndustry) {
        console.log(createdIndustry);
        try {
            const response = await axiosInstance.post('/industry', createdIndustry);
            console.log('Success:', response.data);
            await getIndustries();
        } catch (error) {
            console.error('Error:', error);
        }
    }

    //**************************************/
    //********* Update Industries **********/
    //**************************************/
    async function handleChangedIndustry(editedIndustry, industryId){
        try {
            const response = await axiosInstance.put(`/industry/${industryId}`, editedIndustry);
            console.log('Success:', response.data);
            const index = industries.value.findIndex((industry) => industry.id === editedIndustry.id);
            if(index !== -1){
                industries.value[index] = { ...industries.value[index], ...editedIndustry }; 
            }
        } catch (error) {
            console.error('Error:', error);
        }      
    }
    //**************************************/
    //********* Delete Industries **********/
    //**************************************/
    async function handleDeleteIndustry(IndustryId) {
        try {
            console.log(IndustryId);
            const response = await axiosInstance.delete(`/industry/${IndustryId}`);
            industries.value = industries.value.filter(item => item.id !== IndustryId);
            console.log(response);
        } catch (error) {
            console.error('Error deleting industry')
        }

    }


    //**************************************/
    //********** Filter Companies **********/
    //**************************************/
    
    // Computed Property für die gefilterte Listen von Unternehmen 
    const filteredIndustries = computed(() => {
        if (activeFilter.value === 'name' && searchQueryName.value) {
            // Filter nach Namen 
            return industries.value.filter((industry) =>
                industry.name.toLowerCase().includes(searchQueryName.value.toLowerCase())
            );
        } else if (activeFilter.value === 'id' && searchQueryId.value) {
            // Filter nach Id
            return industries.value.filter((industry) => 
                industry.id == searchQueryId.value
            ); 
        }
        return industries.value;
    }); 
    //*********** Searchbar name ***********/
    const handleSearchQueryName = (query) => {
        searchQueryName.value = query; 
        activeFilter.value = 'name'; 
    }
    //************ Searchbar id ************/
    const handleSearchQueryId = (query) => {
        searchQueryId.value = query

        activeFilter.value = 'id'; 
    }
</script>

<style scoped>
.container-industry-settings-view{
    display: flex;
    flex-direction: column;
    align-content: space-around;
    gap: 5px;

    
}
.section-title {
        font-size: 24px;
        font-weight: bold;
        color: #004d99;
        margin-bottom: 20px;
        border-bottom: 2px solid #2a9d8f;
        padding-bottom: 10px;
    }
</style>