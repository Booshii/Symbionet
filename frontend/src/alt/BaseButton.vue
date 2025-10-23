<script setup>
    import { defineProps, defineEmits } from 'vue'; 
    const props = defineProps({
        btnClass: {
            type: String, 
            default: 'btn-default'
        },
    });
    const emit = defineEmits(['click']); 
    const onClick = (event) => {
        emit('click', event); 
    };

</script>

<template>
    <button :class="btnClass" @click="onClick">
        <slot>Default Button</slot>
    </button>
</template>


<style scoped>
    .btn-default {
        
        padding: 10px 15px;
        
        background-color: #007BFF;
        color: white;
        border: none;
        cursor: pointer;
    }
</style>