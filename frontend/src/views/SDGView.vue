<script setup>
  // axios für die Verbindung zu backend
  import axiosInstance from "@/axios";
  import { ref, reactive, onMounted, computed } from "vue"; 
  import SdgItem from '../components/SdgItem.vue'; 
  import DropDownSelect from '../components/atoms/DropDownSelect.vue'; 

  const activeFilter = ref('all');
  const selectedFilterItem = ref('all')
  const isDoneDropdownOptions = ref([{id: 1, name: "done"}, {id: 2, name: "inprogress"}]);


  // filter-values
  const industries = ref([]);
  const sdgTypes = ref([]);

  // displayed-data
  const sdgTargets = ref([]); 

  // Props for the visibility of the searchbar 

  const props = defineProps({
    showSearch: {
      type: Boolean,
      default: true,
    },
  });

  onMounted(() => {

    getIndustries(); 
    getSdgTypes(); 
    getSdgTargets(); 

  });


  const filteredSdgItems = computed(() => {
    if (activeFilter.value === 'isDoneDropdownSelect' && selectedFilterItem.value?.name === 'done') {
        return sdgTargets.value.filter(item => item.isDone);
    } else if (activeFilter.value === 'isDoneDropdownSelect' && selectedFilterItem.value?.name === 'inprogress') {
        return sdgTargets.value.filter(item => !item.isDone);
    } else if (activeFilter.value === 'industryDropdownSelect' && selectedFilterItem.value) {
        return sdgTargets.value.filter(item => item.industryId === selectedFilterItem.value.id);
    } else if (activeFilter.value === 'sdgDropdownSelect' && selectedFilterItem.value) {
        return sdgTargets.value.filter(item => 
            Array.isArray(item.sdgTypes) && 
            item.sdgTypes.some(sdg => sdg.id === selectedFilterItem.value.id)
        );
    }
    return sdgTargets.value; 
    
    });

	//**************************************/
	//*********** Get Industries ***********/
	//**************************************/
	async function getIndustries() {
		try {
			const response = await axiosInstance.get('/industry');
			industries.value = response.data;
			console.log(industries.value);
		} catch (error) {
			console.error('Error fetching people:', error);
		}
	}

  //**************************************/
	//*********** Get SdgTypes *************/
	//**************************************/
	async function getSdgTypes() {
		try {
			const response = await axiosInstance.get('/sdgtype/simple');
			sdgTypes.value = response.data;
			console.log(response);
		} catch (error) {
			console.error('Error fetching people:', error);
		}
	}

  //**************************************/
	//*********** Get SdgTargets ***********/
	//**************************************/
	async function getSdgTargets() {
		try {
			const response = await axiosInstance.get('/sdg');
			sdgTargets.value = response.data;
			console.log(response);
		} catch (error) {
			console.error('Error fetching people:', error);
		}
	}

  //**************************************/
	//*************** Filter ***************/
	//**************************************/

  function handleSelect({selectedItem, label}){
    if (selectedItem === null){
      console.log("Filter ausgewählt: ", selectedItem);
      activeFilter.value = label;
      selectedFilterItem.value = null;
    }
    console.log("Filter ausgewählt: ", selectedItem);
    console.log(selectedItem.id);
    activeFilter.value = label;
    selectedFilterItem.value = selectedItem;
    // aktion
  }
</script>

<template>
  <div class="sdg-view-container">
    <div v-if="showSearch">
      <h1 class="section-title">SDG-Maßnahmen aller Unternehmen</h1>
    </div>
    <!-- <div class="description-container">
    </div> -->
    <div class="dropdown-container">
      <DropDownSelect 
      :options="sdgTypes"
      :label="'sdgDropdownSelect'"
      :placeholder="'SDG auswählen'"
      @update:selectedItem="handleSelect"
      />
      <DropDownSelect 
        :options="industries"
        :label="'industryDropdownSelect'"
        :placeholder="'Branche auswählen'"
        @update:selectedItem="handleSelect"
      />
      <DropDownSelect 
        :options="isDoneDropdownOptions"
        :label="'isDoneDropdownSelect'"
        :placeholder="'Fertiggestellt'"
        @update:selectedItem="handleSelect"
      />
  </div>

  
    <Suspense>
      <template #default>
        <div class="sdg-targets-container" >
          <SdgItem v-for="item in filteredSdgItems" :key="item.id" :sdgTarget="item" />
        </div>
      </template>
      <template #fallback>
        <div>Loading...</div>
      </template>
    </Suspense>
 
  </div>
</template>

<style scoped>
  .main{
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
  }
  .sdg-view-container{
    display: flex;
    flex-direction: column; 
    /* justify-content: center; */
    align-items: center;
 
    
  }
  .dropdown{
    width: 220px;
    /* padding-left: 20px; */
    padding-bottom: 8px;
  }

  .sdg-targets-container{
    display: grid; 
    grid-template-columns: repeat(3, 1fr); 
    gap: 40px; 
  }
  .dropdown-container{
    display: flex;
    justify-content: center;
    width: 1290px;
    margin-bottom: 40px; 
    gap: 20px; 
   
  }
  /* .dropdown-section{
    display: flex;
    
    justify-content: space-around;
    align-items: flex-end; 
    width: 100%; 
    margin-top: 4px;
    padding-top: 16px; 
    

  } */
  
    select{
        background-color: #00123a;
        color: white;
        cursor: pointer;
        font-size: 14px;
        width: 160px;
        height: 36px;
        border-radius: 4px;
        border: none;
        box-shadow: none;
        text-align: center;        
    }

    .section-title {
        font-size: 36px;
        font-weight: bold;
        color: black;
        margin-bottom: 20px;
        /* border-bottom: 2px solid #2a9d8f; */
        padding-bottom: 10px;
        padding-top: 30px;
    }
  
</style>