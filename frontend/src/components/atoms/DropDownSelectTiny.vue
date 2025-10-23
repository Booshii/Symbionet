<template>
	<div class="dropdown-tiny-container">
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
.dropdown-tiny-container {
	position: relative;
	width: 100%;
}

.dropdown-select {
	width: 100%;
	height: 30px; /* kleiner als vorher */
	border: 1px solid #ccc;
	border-radius: 6px;
	padding: 2px 25px 2px 6px; /* weniger innenabstand */
	font-size: 13px; /* kleinerer Text */
	background-color: white;
	cursor: pointer;
	outline: none;
	appearance: none;
	box-sizing: border-box;
}

/* Pfeil ▼ */
.dropdown-tiny-container::after {
	content: "▼";
	font-size: 10px; /* kleinerer Pfeil */
	color: #555;
	position: absolute;
	right: 10px;
	top: 50%;
	transform: translateY(-50%);
	pointer-events: none;
}

/* Fokus-Stil */
.dropdown-select:focus {
	border-color: #007acc;
	box-shadow: 0 0 2px rgba(0, 122, 204, 0.5);
}

/* Hover-Effekt */
.dropdown-select:hover {
	border-color: #007acc;
}
</style>
