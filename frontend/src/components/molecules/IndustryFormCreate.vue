<template>
    <div class="industry-form-create-container">
        <form @submit.prevent="emitCompany">
            <div class="form-group">
                <label for="title">Titel</label>
                <input type="text" id="title" v-model="createdIndustry.name" required>
            </div>
            <!-- <div class="description">
                <label for="description">Beschreibung</label>
                <input type="text" id="description" v-model="description">
            </div> -->
            <button type="submit" class="create-button">Erstellen</button>
        </form>
    </div>
</template>

<script setup>
    import { ref, defineEmits } from 'vue';

    const emit = defineEmits(['add-industry'])

    const createdIndustry = ref({
        name: '',
    });

    function emitCompany(){
        emit('add-industry', createdIndustry.value);
    }

</script>

<style scoped>
    .create-button {
    background-color: #2a9d8f;
    color: white;
    margin-top: 16px;
    padding: 10px 15px;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    transition: background-color 0.3s;
    }

    .create-button:hover {
    background-color: #21867a;
    }

    .form-group {
        margin-top: 10px;
        display: flex;
        flex-direction: column;
    }
    label {
    font-weight: bold;
    color: #004d99;
    }

    input {
    padding: 10px;
    border: 1px solid #ccc;
    border-radius: 5px;
    transition: border-color 0.3s;
    }
    

</style>