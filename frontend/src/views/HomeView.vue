<template>
  <div class="container-home-view">
    


    <!-- Karte -->
    <div class="map-wrapper">
      <div v-if="loading">Daten werden geladen...</div>
      <div v-else-if="errorOccurred">Fehler beim Laden der Daten. Bitte versuchen Sie es später erneut.</div>
      <Map v-else :companies="companies" />
    </div>

    <!-- Bauchbinde mit Statistiken -->
    <div class="info-strip" v-if="stats">
      <div class="info-box">
        <p class="info-number">{{ stats.goals }}</p>
        <p>Ziele</p>
      </div>
      <div class="info-box">
        <p class="info-number">{{ stats.companies }}</p>
        <p>Unternehmen</p>
      </div>
      <div class="info-box">
        <p class="info-number">{{ stats.industries}}</p>
        <p>Branchen</p>
      </div>
      <div class="info-box">
        <p class="info-number">{{ stats.sdgTargets }}</p>
        <p>Maßnahmen</p>
      </div>
    </div>

    <!-- Überschrift -->
    <div class="hero-top">
      <h1>Willkommen auf der Nachhaltigkeitsplattform</h1>
      <p>
        Diese Plattform unterstützt Sie dabei, Ihre Nachhaltigkeitsziele effektiv umzusetzen.
        Entdecken Sie, wie Ihr Unternehmen zur nachhaltigen Entwicklung beitragen kann.
      </p>
    </div>

    <!-- Angebot -->
    <div class="offer-section">
      <h2>Was wir bieten</h2>
      <div class="offer-cards">
        <div class="card">
          <h3>SDG-Management</h3>
          <p>Verfolgen Sie die Fortschritte Ihrer SDGs. Filtern Sie nach Branchen oder Zielen für eine klare Übersicht.</p>
        </div>
        <div class="card">
          <h3>Materialaustausch</h3>
          <p>Finden Sie verfügbare Abfallprodukte im Netzwerk. Nutzen Sie die Material-ID zur schnellen Zuordnung.</p>
        </div>
        <div class="card">
          <h3>Umweltbilanz (LCA)</h3>
          <p>Vergleichen Sie Materialien mit der JRC ILCD-Datenbank und analysieren Sie ihre Umweltwirkungen.</p>
        </div>
      </div>
    </div>

    <!-- Vision -->
    <!-- <div class="vision-section">
      <h2>Unsere Vision</h2>
      <p>Eine zentrale Plattform für Unternehmen, die Nachhaltigkeit ernst nehmen. Gemeinsam gestalten wir den Wandel hin zu einer kreislauforientierten Wirtschaft.</p>
      <p>Für Fragen oder Anregungen stehen wir Ihnen gerne zur Verfügung.</p>
    </div> -->

    <!-- Weiterleitungen -->
    <!-- 
    
    <div class="redirect-section">
       Weiterleitung 1 
      <div class="redirect-box">
        <div class="redirect-text">
          <h3>Grünes Kraftwerk</h3>
          <p>Erfahren Sie mehr über das Projekt „Grünes Kraftwerk“ und seine Rolle im Nachhaltigkeitsnetzwerk.</p>
          <button class="redirect-button">Weiter &rarr;</button>
        </div>
        <div class="redirect-image">
          <img src="@/assets/gruenes-kraftwerk.jpg" alt="Grünes Kraftwerk" /> 
        </div>
      </div>

      Weiterleitung 2
      <div class="redirect-box">
        <div class="redirect-text">
          <h3>Materialbörse</h3>
          <p>Informieren Sie sich über unsere Plattform zur Wiederverwendung von Materialien im Netzwerk.</p>
          <button class="redirect-button">Zur Materialbörse &rarr;</button>
        </div>
        <div class="redirect-image">
          <img src="@/assets/materialboerse.jpg" alt="Materialbörse" /> 
        </div>
      </div>
    </div> -->

    

  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import axiosInstance from "@/axios";
import Map from '../components/organisms/Map.vue';


const companies = ref([]);
const stats = ref(null); 
const loading = ref(true);
const errorOccurred = ref(false);

onMounted(async () => {
  await getCompanies();    
  await getStatistics(); 
});

async function getCompanies() {
  try {
    const response = await axiosInstance.get('/company/homeview');
    companies.value = response.data;
  } catch (error) {
    console.error('Error fetching companies:', error);
    errorOccurred.value = true; 
  } finally {
    loading.value = false; 
  }
}

// Statistics
async function getStatistics(){
  try {
    const response = await axiosInstance.get('/statistic/home');
    stats.value = response.data;
  } catch (error) {
    console.error('Error fetching statistics:', error);
    errorOccurred.value = true; 
  } finally {
    loading.value = false; 
  }
}

</script>

<style scoped>
.container-home-view {
  display: flex;
  flex-direction: column;
  align-items: center;
  width: 100%;
}

/* Überschrift */
.hero-top {
  text-align: center;
  padding: 60px 20px 40px;
  background-color: #f5f7fa;
  width: 100%;
}
.hero-top h1 {
  font-size: 40px;
  margin-bottom: 20px;
}
.hero-top p {
  font-size: 20px;
  margin: 0 auto;
  max-width: 800px;
}

/* Karte */
.map-wrapper {
  width: 100%;
}

/* Bauchbinde */
.info-strip {
  display: flex;
  justify-content: space-around;
  align-items: center;
  background-color: #e0f5dc;
  padding: 30px 10px;
  width: 100%;
  flex-wrap: wrap;
}
.info-box {
  text-align: center;
  margin: 10px 20px;
}
.info-number {
  font-size: 36px;
  font-weight: bold;
  margin: 0;
}

/* Angebot */
.offer-section {
  max-width: 1200px;
  padding: 60px 20px;
  text-align: center;
}
.offer-section h2 {
  font-size: 32px;
  margin-bottom: 30px;
}
.offer-cards {
  display: flex;
  flex-wrap: wrap;
  gap: 30px;
  justify-content: center;
}
.card {
  background-color: white;
  border-radius: 10px;
  padding: 20px;
  max-width: 300px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
  text-align: left;
}
.card h3 {
  margin-top: 0;
  font-size: 22px;
}
.card p {
  font-size: 16px;
}

/* Vision */
.vision-section {
  background-color: #f0f0f0;
  padding: 60px 20px;
  text-align: center;
}
.vision-section h2 {
  font-size: 28px;
  margin-bottom: 20px;
}
.vision-section p {
  font-size: 18px;
  margin-bottom: 10px;
}

/* Weiterleitungen */
.redirect-section {
  width: 100%;
  padding: 40px 20px;
  display: flex;
  flex-direction: column;
  gap: 40px;
  background-color: #e0f5dc; /* hellgrün */
}
.redirect-box {
  display: flex;
  flex-direction: row;
  background-color: #f1fbe7;
  border-radius: 12px;
  overflow: hidden;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  max-width: 1000px;
  margin: 0 auto;
}
.redirect-text {
  flex: 1;
  padding: 30px;
  display: flex;
  flex-direction: column;
  justify-content: center;
}
.redirect-text h3 {
  font-size: 24px;
  margin-bottom: 10px;
}
.redirect-text p {
  font-size: 18px;
  margin-bottom: 20px;
}
.redirect-button {
  padding: 10px 20px;
  background-color: orangered;
  border: none;
  color: white;
  font-size: 16px;
  border-radius: 8px;
  cursor: pointer;
  width: fit-content;
  transition: background-color 0.3s ease;
}
.redirect-button:hover {
  background-color: darkorange;
}
.redirect-image {
  flex: 1;
  min-height: 200px;
}
.redirect-image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}
</style>
