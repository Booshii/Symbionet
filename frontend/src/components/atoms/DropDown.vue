<!-- 
  * Filename: Dropdown.vue 
  * Description: this component provides a Dropdown Menu with 

-->


<template>
    <div class="container">
        <button @click="toggleDropdown" class="dropdown-button">
            {{ selected ? selected : 'Wähle eine Option' }}
            <span class="caret">&#9660;</span>
        </button>
        <ul v-if="isOpen" class="dropdown-list">
            <li
                v-for="(item, index) in items"
                :key="index"
                :class="itemClass"
                @click="selectItem(item)"
            >
                {{ item.name }}
            </li>
        </ul>

        <!-- <div v-show="isOpen" class="content">
            <div v-for="option in options" :key="option" @click="selectOption(option)" class="dropdown-option">
                {{ option.name }}
            </div>
        </div> -->
    </div>
</template>

<script setup>
    import {computed, defineProps, ref, defineEmits } from 'vue'; 
    const props = defineProps({
        items: {
            type: Array,
            default: () => [],
        },
        buttonText:{
            type: String,
            required: false,
            default: 'Dropdown',
        }
    });

    const emit = defineEmits(['select']); 
    const isOpen = ref(false);
    const selected = ref(null);

    //***** functions ****/
    
    function toggleDropdown() {
        isOpen.value = !isOpen.value;
    }

    function selectItem(item) {
        selected.value = item.name;
        emit('select', item); 
        isOpen.value = false;
    }

    

</script>

<style scoped>
    .container {
        position: relative;
    }

    .dropdown-button {
        padding: 10px 15px;
        background-color: #f0f0f0;
        border: 1px solid #ccc;
        border-radius: 5px;
        cursor: pointer;
        width: 100%;
        text-align: left;
        display: flex;
        justify-content: space-between;
        align-items: center;
        transition: background-color 0.3s, border-color 0.3s;
    }

    .dropdown-button:hover {
        background-color: #e0e0e0;
        border-color: #bbb;
    }

    .caret {
        margin-left: 10px;
        transition: transform 0.3s;
    }

    .dropdown-list {
        position: absolute;
        top: 100%; /* Positioniert die Liste direkt unter dem Button */
        left: 0;
        width: 100%;
        background-color: white;
        border: 1px solid #ccc;
        border-radius: 5px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        margin-top: 5px;
        z-index: 100;
        max-height: 200px;
        overflow-y: auto;
        padding: 0;
        list-style: none;
        transition: opacity 0.3s, transform 0.3s;
    }

    .dropdown-list li {
        padding: 10px 15px;
        cursor: pointer;
        transition: background-color 0.3s;
    }

    .dropdown-list li:hover {
        background-color: #f7f7f7;
    }

    .dropdown-list li:active {
        background-color: #e0e0e0;
    }

    .dropdown-list li:not(:last-child) {
        border-bottom: 1px solid #e0e0e0;
    }

    .dropdown-option {
        padding: 8px 12px;
        transition: background-color 0.3s, color 0.3s;
    }

    .dropdown-option:hover {
        background-color: #ddd;
        color: #333;
    }
</style>