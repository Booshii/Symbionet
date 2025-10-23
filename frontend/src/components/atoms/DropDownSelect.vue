<template>
	<div class="dropdown-container">
		<select 
			:name="label + '-select'"
			:id="label + '-select'"
			v-model="selectedValue" 
			@change="emitSelection($event)"
			class="dropdown-select"
		>
			<!-- erste Auswahloption -->
			<option :value="null">kein Filter</option>

			<!-- Platzhalter -->
			<option value="" disabled hidden>{{ placeholder }}</option>  

			<!-- Dynamische Optionen -->
			<option v-for="(option, index) in options" :key="index" :value="option.id">
				{{ option.id }} - {{ option.name }}
			</option>
		</select>
	</div>
</template>

<script setup>
import { defineProps, defineEmits, ref } from 'vue'; 

const props = defineProps({
	options: {
		type: Array,
		required: true,
		default: () => [],
	}, 
	placeholder: {
		type: String,
	},
	label: {
		type: String,
		required: true
	}
});

const emit = defineEmits(['update:selectedItem']);
const selectedValue = ref("");

const emitSelection = (event) => {
	if (selectedValue.value === null) {
		emit('update:selectedItem', { selectedItem: null, label: props.label });
		return;
	}
	const selectedId = event.target.value; 
	const selectedItem = props.options.find(option => option.id === Number(selectedId));
	if (selectedItem) {
		emit('update:selectedItem', { selectedItem, label: props.label }); 
	}
};
</script>

<style scoped>
/* Container */
.dropdown-container {
	position: relative;
	width: 100%;
}

/* Dropdown-Stil (angepasst an Suchleiste) */
.dropdown-select {
	width: 100%;
	height: 40px; /* Gleiche Höhe wie Suchleiste */
	border: 1px solid grey;
	border-radius: 10px;
	padding: 5px 35px 5px 10px; /* Rechts Platz für Pfeil */
	font-size: 14px;
	background-color: white;
	cursor: pointer;
	outline: none;
	appearance: none;
}

/* Eigenes Dreieck (Pfeil ▼) */
.dropdown-container::after {
	content: "▼"; /* Unicode-Pfeil */
	font-size: 14px;
	color: black;
	position: absolute;
	right: 15px;
	top: 50%;
	transform: translateY(-50%);
	pointer-events: none; /* Klicks gehen trotzdem auf das Dropdown */
}

/* Fokus-Stil */
.dropdown-select:focus {
	border-color: #21867a;
	box-shadow: 0 0 4px rgba(33, 134, 122, 0.5);
}

/* Hover-Effekt */
.dropdown-select:hover {
	border-color: #004d99;
}
</style>
