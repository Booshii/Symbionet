<script setup>
    import { ref } from 'vue';


    const subGoals = ref([]); // speichert alle subGoals

    // Funktion, die ein neues SubGoal-Objekt zum Array hinzufügt 
    function addSubGoal(){
        subGoals.value.push({
            title: '',
            description: ''
        });
    }
    const showAddSubGoal = ref(false); // Reaktiver Zustand für das Ein-/Ausblenden
    function handleClick() {
        showAddSubGoal.value = !showAddSubGoal.value; // Wechseln des Zustands beim Klicken
    }

    // Funktion, um den Titel oder die Beschreibung eines SubGoals zu aktualisieren
    // nochmal überdenken 
    function updateSubGoal(index, field, value) {
        subGoals.value[index][field] = value;
    }

    function removeSubGoal(index) {
        subGoals.value.splice(index, 1);
    }


    function getData() {
        return subGoals.value;
    }

    defineExpose({
        getData
    });

</script>


<template>
    <div class="addSubGoal">
        

        <div v-for="(subGoal, index) in subGoals" :key="index">
            <transition name="fade" mode="out-in">
                <div class="addSubGoal">
                    
                    <div class="title-area">
                        <label :for="'title-' + index" class="title-label">Titel</label>
                        <button @click="removeSubGoal(index)" class="close-button">&#10005;</button>
                    </div>
                    <input type="text" :id="'title-' + index" v-model="subGoals[index].title" required>
                    <div class="description">
                        <label :for="'description-' + index">Teilziel-Beschreibung</label>
                        <textarea :id="'description-' + index" v-model="subGoals[index].description" required></textarea>
                    </div>
                </div>
            </transition>
        </div>


        <button @click="addSubGoal" class="clickable-area">
            <!-- Hier ein Plus mit Text -->
            Neues Teilziel hinzufügen
        </button>



          <!-- <transition name="fade" mode="out-in">
            <div v-if="showAddSubGoal" key="subgoals" class="">
                <div class="title">
                    <label for="title">Titel</label>
                    <input type="text" id="title" v-model="title" required>
                </div>
                <div class="description">
                    <label for="description">Ziel-Beschreibung</label>
                    <textarea type="text" id="description" v-model="description" required></textarea>
                </div>
            </div>
          </transition> -->
        </div>
</template>


<style scoped>
    .addSubGoal * {
        /* border-style: solid; */
        display: flex;
        flex-direction: column;
    }
    .clickable-area{
        cursor: pointer;
        padding: 10px;
        background-color: hsl(211, 18%, 80%);
        border-radius: 4px;
        border: 1px solid #ccc;
        margin-bottom: 10px;
    }
    .title-area{
        display: flex;
        flex-direction: row; 
        align-items: center;
        justify-content: space-between;
    }
    .title-label {
        flex: 1; /* Ermöglicht dem Label, den verfügbaren Platz auszufüllen */
    }
    .close-button {
        cursor: pointer;
        border: none;
        background-color: transparent;
        color: red;
        font-size: 24px;
        padding: 0 10px;
        margin-left: 20px; /* Gibt etwas Abstand zwischen Label und Button */
    }
    .subGoal-item + .subGoal-item {
        margin-top: 20px; /* Fügt Abstand zwischen SubGoal-Elementen hinzu */
    }
    input{
        height: 32px; 

        border-style: solid;
        border-color: black;
        border-width: 1px;
        border-radius: 6px;
    }
    .description textarea{
        height: 90px; 
        border-style: solid;
        border-color: black;
        border-width: 1px;
        border-radius: 6px;
    }

</style>