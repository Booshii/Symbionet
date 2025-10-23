<template>
  <!-- <div class="container-company-sdgTargets-list" v-for="item in filteredSdgTargets" :key="item.id" >
    <div class="list-header">
      <div class="title-area">
        <h1> {{ sdgType.number }} {{ sdgType.title }} </h1>
        <p>22.10.2024</p>
      </div>
      <div class="description-area">
        <p> 
         Hier habe ich vergessen in der Datenbank eine Beschreibung hinzuzufgügen 
        </p>
      </div>
    </div> -->

    <!-- <SdgTargetItem 
      v-for="sdgTarget in filteredSdgTargets"  
      :key="sdgTarget.id" 
      :sdgTarget="sdgTarget"
    /> -->
  
  <div class="dropdown-container">
    <DropDownSelect 
    :options="sdgTypes"
    :label="'sdgDropdownSelect'"
    :placeholder="'SDG auswählen'"
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

  <!-- </div> -->
</template>
<script setup>

    import { ref, reactive, onMounted, computed } from "vue"; 
    import DropDownSelect from "../atoms/DropDownSelect.vue";
    import Color from 'color'; 
    import SdgItem from '../SdgItem.vue';

    const props = defineProps({
      sdgTargets: {
        type: Array,
        // default() {
        //   return [ 
        //     {
        //       id: '0',
        //       title: "default title",
        //       description: "default Description",
        //       isDone: false,
        //       companyName: "default CompanyName",
        //       sdgTypeColors: ["hsl(0,0%,50%)"],
        //       subGoals:[]
        //     }
        //   ];
        // }
      },
      sdgTypes: {
        type: Array,
        required: true
              
      }
    });


    const activeFilter = ref("all"); 
    const selectedFilterItem = ref('all')
    const isDoneDropdownOptions = ref([{id: 1, name: "done"}, {id: 2, name: "inprogress"}]);



  //**************************************/
	//*************** Filter ***************/
	//**************************************/

  const filteredSdgItems = computed(() => {
  if (activeFilter.value === "isDoneDropdownSelect" && selectedFilterItem.value?.name === "done") {
    return props.sdgTargets.filter(item => item.isDone);
  } else if (activeFilter.value === "isDoneDropdownSelect" && selectedFilterItem.value?.name === "inprogress") {
    return props.sdgTargets.filter(item => !item.isDone);
  } else if (activeFilter.value === "sdgDropdownSelect" && selectedFilterItem.value) {
    return props.sdgTargets.filter(item => 
      Array.isArray(item.sdgTypes) && 
      item.sdgTypes.some(sdg => sdg.id === selectedFilterItem.value.id)
    );
  }
  return props.sdgTargets;
});

    // Auswahl aus dem Dropdown verarbeiten 

    function handleSelect({ selectedItem, label}){
      if (selectedItem === null){
        console.log("Filter ausgewählt: ", selectedItem);
        activeFilter.value = label;
        selectedFilterItem.value = null;
      }
      activeFilter.value = label; 
      selectedFilterItem.value = selectedItem; 
    }




    // gucken 

    //Color Settings
    // const color = props.sdgType.value.color;
    // const lighterColor = Color(color).lighten(0.7).string();
    // onMounted(() => {
      

      
    // });
    






</script>
<style scoped>
  .container-company-sdgTargets-list{
    width: 100%;
    max-width: 1200px;
    background-color: v-bind(lighterColor);
    padding: 28px 40px;
    margin: 16px;

  }
  .title-area{
    display: flex;
    flex-direction: row; 
    justify-content: space-between;
    padding: 0px 40px;
    
    color: white;
    background-color: v-bind(color);
  }

  .description-area{
    width: 100%;
    
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

</style>
  