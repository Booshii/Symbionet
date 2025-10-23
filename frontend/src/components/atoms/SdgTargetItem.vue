<template>
    <div class="sdg-target-item-container">
        <div class="title" @click="toggleDescription">
            <h2>{{ sdgTarget.title }}</h2>
            <span :class="{'button-arrow': true, 'rotated': showDescription}">&#10095;</span>
            
        </div>
        <div v-if="showDescription" class="description" >
            <p>{{ sdgTarget.description }}</p>
        </div>
    </div>
</template>

<script setup>
    import { ref } from 'vue'; 

    const props = defineProps({
        sdgTarget: {
            type: Object
        }
    })

    const showDescription = ref(false); 

    function toggleDescription(){
        showDescription.value = !showDescription.value;
    }
</script>

<style scoped>
    .sdg-target-item-container{
        display: flex;
        flex-direction: column;
        align-content: center;

        
        border-top: 2px solid ;
    }
    .title{
        display: flex;
        flex-direction: row;
        justify-content: space-between;
        align-content: center;

        width: 100%;
        
    }
    
    .button-arrow {
        transition: transform 0.3s ease-in-out;
        align-content: center;
    }

    .rotated {
        transform: rotate(90deg);
    }
</style>
