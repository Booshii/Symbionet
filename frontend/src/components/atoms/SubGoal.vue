<script setup>
import { ref } from 'vue';


// Zustand für das Dropdown
const isOpen = ref(false);
function toggleDropdown() {
    isOpen.value = !isOpen.value;
}

// Definiere Props
const props = defineProps({
    SubGoal: {
        type: Object,
        required: true,
    },
    color: {    // Farbe von Parent
        type: String,
        default: '#000' 
    }
});

// Debugging: Ausgabe der SubGoal in der Konsole
console.log(props.SubGoal);
</script>

<template>
    <div class="sub-goal-container" >
        <button class="custom-button" @click="toggleDropdown">
            <span class="button-text"> {{ SubGoal.title }} </span>
            <span :class="{'button-arrow': true, 'rotated': isOpen}">&#10095;</span>
        </button> 
        <p class="sub-goal-description" v-if="isOpen"> {{ SubGoal.title }} </p>
        
    </div>
</template>

<style scoped>
    .sub-goal-container {
        display: flex;
        flex-direction: column;
        padding: 20px 40px;

        border-top: 1px solid; 
        
        background-color: v-bind(color);
    }
    .custom-button{
        display: flex;
        justify-content: space-between;

        padding: 0px;
        border-style: none;
        width: 100%;
        background-color: v-bind(color);
        color: black;
        
        cursor: pointer;
    }
    .button-text{
        font-weight: bold;
        font-size: 18px
    }
    .sub-goal-description{
        display: flex;  
    }
    
    .button-arrow {
        transition: transform 0.3s ease-in-out;
    }

    .rotated {
        transform: rotate(90deg);
    }

    
</style>



