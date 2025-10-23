<template>
  <div class="container-map-popup">
    <h3 @click="navigateToDetail(company)" style="cursor: pointer;">{{ company.name }}</h3>
    <p>{{ company.street }} {{ company.streetNumber }}, {{ company.city }}</p>
    <p>Erfüllte Ziele:</p>
    <div style="display: flex; flex-wrap: nowrap; height: 40px; width: 40px">
      <img v-for="(img, index) in sdgImages" :key="index" :src="img" alt="SDG logo" style="height: 40px; width: 40px; margin-right: 10px"/>
    </div>
  </div>
</template>

<script setup>
import { useRouter } from 'vue-router';
import { defineProps } from 'vue';

const router = useRouter();

const props = defineProps({
  company: {
    type: Object,
    required: true
  },
  sdgImages: {
    type: Array,
    required: true
  }
});


// Methode für die Navigation
function navigateToDetail(company) {
    router.push({ name: 'CompanyView', params: { id: company.id } });
}
</script>

<style lang="scss" scoped>

</style>  