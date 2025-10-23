<template>
	<div class="container-map-organism">
		<div class="dropdown">
			<DropDownSelect 
				:options="industries"
				:label="'industryDropdownSelect'"
				:placeholder="'Branche auswählen'"
				@update:selectedItem="handleSelect"
			/>
		</div>
		<div class="sdg-filter">
			<div class="img-column-first">
				<img src="@/assets/sdg-logos/sdg1_logo.svg" alt="sdg-1" style="cursor: pointer;">
				<img src="@/assets/sdg-logos/sdg2_logo.svg" alt="sdg-2">
				<img src="@/assets/sdg-logos/sdg3_logo.svg" alt="sdg-3">
				<img src="@/assets/sdg-logos/sdg4_logo.svg" alt="sdg-4">
				<img src="@/assets/sdg-logos/sdg5_logo.svg" alt="sdg-5">
				<img src="@/assets/sdg-logos/sdg6_logo.svg" alt="sdg-6">
				<img src="@/assets/sdg-logos/sdg7_logo.svg" alt="sdg-7">
				<img src="@/assets/sdg-logos/sdg8_logo.svg" alt="sdg-8">
			</div>
			<div class="img-column-second">					
				<img src="@/assets/sdg-logos/sdg9_logo.svg" alt="sdg-9">
				<img src="@/assets/sdg-logos/sdg10_logo.svg" alt="sdg-10">
				<img src="@/assets/sdg-logos/sdg11_logo.svg" alt="sdg-11">
				<img src="@/assets/sdg-logos/sdg12_logo.svg" alt="sdg-12">
				<img src="@/assets/sdg-logos/sdg13_logo.svg" alt="sdg-13">
				<img src="@/assets/sdg-logos/sdg14_logo.svg" alt="sdg-14">
				<img src="@/assets/sdg-logos/sdg15_logo.svg" alt="sdg-15">
				<img src="@/assets/sdg-logos/sdg16_logo.svg" alt="sdg-16">
				<img src="@/assets/sdg-logos/sdg17.svg" alt="sdg-17">
			</div>
		</div>
		<div class="map-container">
			<div id="map" ref="mapElement"></div>
		</div>
	</div>

</template>

<script setup>
import { ref, onMounted, computed } from 'vue';
import L from 'leaflet';
import 'leaflet/dist/leaflet.css';
import DropDownSelect from '../atoms/DropDownSelect.vue';
import { useRouter } from 'vue-router';
import axiosInstance from '@/axios';

const router = useRouter();

const props = defineProps({
  companies: {
    type: Array,
    required: false,
  },
});

const southWest = L.latLng(52.3, 13.0);
const northEast = L.latLng(52.7, 13.7);
const bounds = L.latLngBounds(southWest, northEast);

const mapElement = ref(null);
const mapInstance = ref(null);
const markers = ref([]);

const industries = ref([]);
const activeFilter = ref('all');
const selectedFilterItem = ref(null);

onMounted(() => {
  mapInstance.value = initializeMap();
  getIndustries();
  updateMarkers();
});

function initializeMap() {
  const map = L.map(mapElement.value, {
    center: [52.405821, 13.380030],
    zoom: 15,
    maxBounds: bounds,
    maxZoom: 16,
    minZoom: 10,
    scrollWheelZoom: false
  });

  L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; OpenStreetMap contributors'
  }).addTo(map);

  map.on('click', () => map.scrollWheelZoom.enable());
  map.on('mouseout', () => map.scrollWheelZoom.disable());

  return map;
}

const filteredCompanies = computed(() => {
  if (activeFilter.value === 'industryDropdownSelect' && selectedFilterItem.value) {
    return props.companies.filter(
      company => company.industryId === selectedFilterItem.value.id
    );
  }
  return props.companies;
});


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
  17: new URL('@/assets/sdg-logos/sdg17.svg', import.meta.url).href,
};



function updateMarkers() {
  if (!mapInstance.value) return;
  markers.value.forEach(marker => marker.remove());
  markers.value = [];

  filteredCompanies.value.forEach(company => {
    if (!company.latitude || !company.longitude || isNaN(company.latitude) || isNaN(company.longitude)) return;

    let imgHtml = '';
    if (company.sdgTypes && Array.isArray(company.sdgTypes)) {
      const uniqueIds = [...new Set(company.sdgTypes.map(t => t.id))].sort((a, b) => a - b);
      for (const id of uniqueIds) {
        if (imgUrls[id]) {
          imgHtml += `<img src="${imgUrls[id]}" alt="SDG ${id}" style="height: 40px; width: 40px; margin-right: 10px;" />`;
        }
      }
    }

    const popupContent = `
      <h3 style="cursor: pointer;" onclick="window.__navigateToCompany(${company.id})">${company.name}</h3>
      <p>${company.street} ${company.streetNumber}, ${company.city}</p>
      <p>Erfüllte Ziele:</p>
      <div style="
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(40px, 1fr));
        max-width: 265px;
        gap: 5px;
        justify-content: start;
      ">
        ${imgHtml}
      </div>
    `;

    const marker = L.marker([company.latitude, company.longitude]).addTo(mapInstance.value);
    marker.bindPopup(popupContent);
    markers.value.push(marker);
  });
}
window.__navigateToCompany = function (id) {
  router.push({ name: 'CompanyView', params: { id } });
};

function handleSelect({ selectedItem, label }) {
  activeFilter.value = label;
  selectedFilterItem.value = selectedItem;
  updateMarkers();
}

async function getIndustries() {
  try {
    const response = await axiosInstance.get('/industry');
		console.log(response);
    industries.value = response.data;
  } catch (error) {
    console.error('Fehler beim Abrufen der Branchen:', error);
  }
}



</script>

<style scoped>
.container-map-organism {
  width: 100%;
}
.sdg-filter {
  position: absolute;
  z-index: 1000;
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  padding: 12px;
  width: 100%;
  pointer-events: none;
}
.sdg-filter img {
  width: 70px;
  height: 70px;
}
.img-column-first, .img-column-second {
  display: flex;
  flex-direction: column;
  gap: 10px;
}
.img-column-first { padding-top: 100px; }
.img-column-second { padding-top: 20px; }
.sdg-filter img, .dropdown {
  pointer-events: auto;
}
.dropdown {
  position: absolute;
  top: 100px;
  left: 80px;
  z-index: 1000;
}
.map-container {
  position: relative;
  width: 100%;
  height: 900px;
}
#map {
  width: 100%;
  height: 100%;
}
</style>
