<script setup>
	import { ref,computed } from 'vue'; 
	import { reactive } from 'vue';
	import Color from 'color'; 

	import BaseButton from './atoms/Button.vue';
	import SubGoal from './atoms/SubGoal.vue';

	import tickIcon from '@/assets/tick-box.svg';
	import planningIcon from '@/assets/empty-box.svg';

	const props = defineProps({
	sdgTarget: {
		type: Object,
		required: true,
	},
	btnClass: {
		type: String,
		default: 'btn-default'
	}
	});

	// Script Button 
	const showSdgIcons = ref(false); // Reaktiver Zustand für das Ein-/Ausblenden der SubGoals 
	const toggleSdgIcons = () => {
		showSdgIcons.value = !showSdgIcons.value; // Wechseln des Zustands beim Klicken
	};

	// Button-Text basierend auf dem Status
	const buttonText = computed(() => showSdgIcons.value ? 'Zugehörige SDGs ausblenden' : 'Zugehörige SDGs anzeigen');

	// SdgColor 
	const color = props.sdgTarget.sdgTypes[0].color;
	const lighterColor = Color(color).lighten(0.6).string();
	const descriptionBackground = Color(color).lighten(0.7).string();
	console.log(props.sdgTarget);
	


	const imgUrls = {
		1: new URL('@/assets/sdg-logos/sdg1_logo.svg', import.meta.url).href,
		2: new URL('@/assets/sdg-logos/sdg2_logo.svg', import.meta.url).href,
		3: new URL('@/assets/sdg-logos/sdg3_logo.svg', import.meta.url).href,
		4: new URL('@/assets/sdg-logos/sdg4_logo.svg', import.meta.url).href,
		5: new URL('@/assets/sdg-logos/sdg5_logo.svg', import.meta.url).href,
		6: new URL('@/assets/sdg-logos/sdg6_logo.svg', import.meta.url).href,
		7: new URL('@/assets/sdg-logos/sdg7_logo.svg', import.meta.url).href,
		8: new URL('@/assets/sdg-logos/sdg8_logo.svg', import.meta.url).href,
		9: new URL('@/assets/sdg-logos/sdg9_logo.svg', import.meta.url).href,
		10: new URL('@/assets/sdg-logos/sdg10_logo.svg', import.meta.url).href,
		11: new URL('@/assets/sdg-logos/sdg11_logo.svg', import.meta.url).href,
		12: new URL('@/assets/sdg-logos/sdg12_logo.svg', import.meta.url).href,
		13: new URL('@/assets/sdg-logos/sdg13_logo.svg', import.meta.url).href,
		14: new URL('@/assets/sdg-logos/sdg14_logo.svg', import.meta.url).href,
		15: new URL('@/assets/sdg-logos/sdg15_logo.svg', import.meta.url).href,
		16: new URL('@/assets/sdg-logos/sdg16_logo.svg', import.meta.url).href,
		17: new URL('@/assets/sdg-logos/sdg16_logo.svg', import.meta.url).href,
	}
	




</script>


<template>
	
	<div class = "container-sdg-item">
			
		<div class="title-area">
			<!-- <h1> {{  sdgTarget.title  }} </h1>  -->
			<h1> {{  sdgTarget.title  }} </h1> 
			<p>22.10.2024</p>           
		</div>
		<!-- Description -->
		<div class="description-area">
			<p id="description"> {{ sdgTarget.description }}</p>
			<div class="company-isdone-area">
				<p> {{ sdgTarget.companyName }}</p>
				
				<div style="display: flex; gap: 3px; align-items: center;">
					<p>{{ sdgTarget.isDone ? 'Durchgeführt' : 'In Planung' }}</p>
					<img
						:src="sdgTarget.isDone ? tickIcon : planningIcon"
						:alt="sdgTarget.isDone ? 'durchgeführt' : 'in planung'"
						style="height: 14px; width: 14px; margin-right: 10px;"
					>
					<!-- <p>Durchgeführt</p>
					<img src="@/assets/tick-box.svg" alt="tick-box" style="height: 10px; width: 10px; margin-right: 10px;"> -->
				</div>
			
			
			
			</div>
		</div>

			<div v-if="showSdgIcons" class="sdg-icons-area">
				<!-- <p>zugehörige Sdg's</p> -->
				<div v-for="(sdgType) in sdgTarget.sdgTypes" :key="sdgType.id"
				class="sdg-icons">
					<img v-if="imgUrls[sdgType.id]" :src="imgUrls[sdgType.id]" :alt="'SDG ' + sdgType.id"
						style="height: 40px; width: 40px; margin-right: 10px;" />
				</div>
			</div>
				
			<BaseButton class="sdgTarget-button" @click="toggleSdgIcons"> 
					<slot> {{ buttonText }} </slot> 
			</BaseButton>
	</div>
	
	<!-- <div v-if="showSdgIcons">
			
				<SubGoal v-for="subGoal in sdgTarget.subGoals" :key="subGoal.id" :SubGoal="subGoal" :color="lighterColor" /> 
	</div> -->

	
	
</template>


<style scoped>
	
	.container-sdg-item{
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		min-width: 520px;
		max-width: 520px;
		min-height: 160px;
		background-color: v-bind(lighterColor);
		padding: 20px;
		gap: 10px; 
		box-sizing: border-box;

	}
	.title-area{
			display: flex;
			flex-direction: row; 
			justify-content: space-between;
			width: 100%;
			padding: 0 24px;
			color: v-bind(lighterColor);
			background-color: v-bind(color);
			font-size: 1cap;
	}
	.description-area{
		display: flex;
		flex-direction: column;
		justify-content: space-between;
		font-size: 1cap;
		min-height: 80px; 
		min-width: 440px;
		width: 100%;
		/* background-color: v-bind(descriptionBackground); */
		
					
	}

	#description{
			font-size: 18px;
			color: v-bind(color);
			font-weight: 600;
			padding: 0 30px;
	}
	
	.sdgTarget-button{
			
			width: 100%;
			border-top: 1px solid black;
			background-color: v-bind(lighterColor);
			font-size: 1cap;
			color: black;
	}
	.sdg-icons{
		display: flex; 
		flex-direction: row;
		justify-content: flex-start;
		background-color: v-bind(lighterColor);
	}
	.company-isdone-area{
			display: flex; 
			justify-content: space-between;
	}

	.sdg-icons-area{
		display: flex; 
		flex-direction: row;
		justify-content: flex-start;
		width: 100%;
	}

</style>

