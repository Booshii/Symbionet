<template>
    <div class="container-company-overview">
        <h1>Mitgliedsunternehmen</h1>

        <div class="searchbar-container">
            <Searchbar 
                class="searchbar"
                :placeholder="'Suchbegriff eingeben'" 
                @update:search="handleSearchQueryName" 
            />
        </div>        

        <div class="table-container">
            <table>
                <thead>
                    <tr>
                        <th>Unternehmen</th>
                        <th>Branche</th>
                        <th>Bundesland</th>
                        <th>PLZ</th>
                    </tr>
                </thead>
                <tbody>
                    <template v-for="(group, letter) in groupedCompanies" :key="letter">
                        <tr class="group-row">
                            <td colspan="4">
                                <h2>{{ letter }}</h2>
                            </td>
                        </tr>
                        <tr v-for="company in group" :key="company.id" @click="navigateToDetail(company)">
                            <td>{{ company.name }}</td>
                            <td>{{ company.industryName }} </td>
                            <td>{{ company.city }}</td>
                            <td>{{ company.postalcode }}</td>
                        </tr>
                    </template>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script setup>
import Searchbar from '../components/atoms/Searchbar.vue'; 
import { onMounted, ref, computed } from 'vue';   
import { useRouter } from 'vue-router'; 
import axiosInstance from "@/axios";

const router = useRouter(); 
const companies = ref([]); 
const activeFilter = ref('name'); 
const searchQueryName = ref(''); 

const filteredCompanies = computed(() => {
    if (activeFilter.value === 'name') {
        return companies.value.filter((company) => 
            company.name.toLowerCase().includes(searchQueryName.value.toLowerCase())
        );
    }
    return companies.value;
});

const groupedCompanies = computed(() => {
    const sortedCompanies = [...filteredCompanies.value].sort((a, b) => 
        a.name.localeCompare(b.name)
    );

    const groups = {}; 
    sortedCompanies.forEach((company) => {
        const firstLetter = company.name.charAt(0).toUpperCase(); 
        if (!groups[firstLetter]) {
            groups[firstLetter] = [];
        }
        groups[firstLetter].push(company);
    });

    return groups; 
});

onMounted(() => {
    getCompanies(); 
});

async function getCompanies(){
    try {
        const response = await axiosInstance.get('/company/'); 
        companies.value = response.data; 
    } catch (error) {
        console.error('Error fetching companies:', error); 
    }
}; 

const handleSearchQueryName = (query) => {
    searchQueryName.value = query; 
    activeFilter.value = 'name'; 
}

function navigateToDetail(company) {
    router.push({ name: 'CompanyView', params: { id: company.id } });
}
</script>

<style scoped>
/* Container-Stil für die gesamte Ansicht */
.container-company-overview {
    display: flex;
    flex-direction: column;
    align-items: center;
    width: 80vw;
    padding: 20px;
    /* background-color: #f5f9fc; */
    border-radius: 10px;
    /* box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); */
}

/* Hauptüberschrift */
h1 {
    color: black;
    margin-bottom: 20px;
}

/* Tabellen-Container */
.table-container {
    width: 100%;
    overflow-x: auto;
}

/* Tabelle */
table {
    border-collapse: collapse;
    width: 100%;
    font-family: ui-sans-serif, sans-serif;
    background-color: #ffffff;
    border-radius: 20px;
    border: 1px solid #e1e8ed;
    overflow: hidden;
}

/* Tabellenkopf */
thead {
    /* background-color: #2a6496; */
    color: black;
}

/* Spalten-Header */
th {
    padding: 12px 15px;
    text-align: left;
    font-family: ui-sans-serif, sans-serif;
/* 
    text-transform: uppercase;
    letter-spacing: 0.1em; */
    height: 50px;
}

/* Tabellenzellen */
td {
    padding: 12px 15px;
    border-bottom: 1px solid #e1e8ed;
    text-align: left;
    color: #333;
    height: 50px;
}

/* Alternierende Zeilenfarben */
tbody tr:nth-child(even) {
    background-color: #f2f2f2;
}

tbody tr:nth-child(odd) {
    background-color: #ffffff;
}

/* Hover-Effekt für klickbare Zeilen */
tbody tr:hover {
    background-color: #e8f6f9;
    cursor: pointer;
}

/* Überschriften innerhalb der Tabelle */
h2 {
    margin: 0;
    padding: 10px;
    font-size: 1.3em;
    color: grey;
    /* background-color: #f3faff; */
    text-align: left;
}

/* Gruppenüberschriften (z. B. Buchstaben A-Z) */
.group-row {
    pointer-events: none;
    /* background-color: #f3faff; */
    cursor: default;
}

/* Erstes Element in jeder Zeile fett */
tbody tr td:first-child {
    /* font-weight: bold; */
    /* color: #2a6496; */
}

/* Suchleiste */
.searchbar-container {
    width: 50%;
    margin-bottom: 20px;
}

/* Suchfeld */
.searchbar {
    width: 100%;
    height: 50px;
    padding: 0 15px;
    /* border: 1px solid #ccc; */
    border-radius: 10px;
    font-size: 1em;
    box-sizing: border-box;
}
</style>
