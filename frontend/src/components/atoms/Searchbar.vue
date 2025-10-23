<!-- 
  * Filename: Searchbar.vue 
  * Description: this component provides a Searchbar  
  *              it takes an array from the parent component with the options 
  *              and provides the selected option back 
                 @input wird bei jeder Änderung ausgeführt
  * Props: 
  *         options: - list of selectable options
  * Emits: 
  *         selectedOption.value    
-->

<template>
    <div class="searchbar-container">
        <!-- <label for="search-item">{{ label }}</label> -->
        <div class="search-input">
            <span class="search-icon">
                <img class="pictogram" src="../../assets/svg-emoticons/magnifying-glass.svg" alt="Lupe">
            </span>
            <input 
            type="search" 
            class="input"
            id="search-item" 
            name="search-item"
            :placeholder= "placeholder"
            :value = inputValue
            @input="emitSearchQuery($event.target.value)"
            autocomplete="off"
            >
        </div>  
    </div>
</template>

<script setup>
    import { defineEmits, defineProps } from 'vue'; 
    
    const props = defineProps({
        placeholder: {
            type: String,
            required: false,
        },
        label: {
            type: String,
            required: false,
        },
        inputValue: {
            type: String,
            required: false,
        }


    })
    const emit = defineEmits(['update-search']);
    const emitSearchQuery = (value) => {
        emit('update-search', value);
    }
    
</script>

<style scoped>
    .searchbar-container {
    display: flex;
    width: 100%;
}

/* Suchleisten-Styling */
.search-input {
    display: flex;
    align-items: center;
    border: 1px solid grey;
    border-radius: 10px;
    padding: 5px 10px;
    width: 100%;
    background-color: white;
    height: 40px; /* Gleiche Höhe wie Tabellenzeilen */
}

/* Suchsymbol */
.search-icon img {
    width: 20px;
    height: 20px;
}

/* Eingabefeld */
.input {
    border: none;
    outline: none;
    width: 100%;
    font-size: 14px;
    height: 30px;
    padding-left: 5px;
}

</style>