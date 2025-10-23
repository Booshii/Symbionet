
<!-- <button v-if="showEditRowId === sdgTarget.id" @click="saveChanges(sdgTarget.id)" class="edit-button">Speichern</button>
                            <button v-else @click="showEditRow(sdgTarget)" class="edit-button">Bearbeiten</button> -->

<template>
    <button
        :type="type"
        :class="['button', variant]"
        v-bind="$attrs"
    >
        <slot></slot>
    </button>
</template>
1
<script setup>
    import { defineProps } from 'vue'; 

    const props = defineProps({
        // can set to 'button', 'submit' or 'reset'. 
        type: {
            type: String,
            default: 'button',
        },
        // can set to 'default', 'save', 'edit' or 'delete'. 
        variant: {
            type: String,
            default: 'default',
        }
    });
</script>

<style scoped>
    .button{
        padding: 8px 16px;
        border: none;
        border-radius: 4px; 
        background-color: #007bff; 
        color: #fff; 
        cursor: pointer;
    }
    .default {
        background-color: #2a9d8f;
        color: white; 
    }

    .save {
        background-color: #004d99;
        color: white; 
    }

    .edit{
        background-color: #2a9d8f;
        color: white; 
    }
    
    .delete{
        background-color: red;
        color: white; 
    }

    .button:hover {
        background-color: #0056b3
    }
</style>