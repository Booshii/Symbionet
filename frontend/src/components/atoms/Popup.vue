<template>
    <div class="popup-overlay">
        <div class="popup-container">
            <slot></slot>
            <Button variant="default" @click="confirm()">Ja</Button>
            <Button variant="default" @click="cancel()">Nein</Button>
        </div>
    </div>
    
</template>

<script setup>
    import { defineProps, defineEmits } from 'vue';

    import Button from './Button.vue'; 


    const emit = defineEmits(['confirm', 'cancel']);

    function confirm(){
        emit('confirm');
    }
    function cancel(){
        emit('cancel');
    }

</script>

<style scoped>
    /* Hintergrund-Overlay für das Popup */
.popup-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5); /* Transparenter schwarzer Hintergrund */
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000; /* Stellt sicher, dass das Popup über allem liegt */
}

/* Container für das eigentliche Popup */
.popup-container {
  background: white;
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.3);
  text-align: center;
  min-width: 300px;
  max-width: 400px;
}

/* Stile für die Buttons */
.popup-buttons {
  display: flex;
  justify-content: space-around;
  margin-top: 20px;
}

.popup-button {
  padding: 10px 20px;
  border: none;
  border-radius: 5px;
  cursor: pointer;
}

.confirm {
  background-color: #d9534f;
  color: white;
}

.confirm:hover {
  background-color: #c9302c;
}

.cancel {
  background-color: #5bc0de;
  color: white;
}

.cancel:hover {
  background-color: #31b0d5;
}
</style>