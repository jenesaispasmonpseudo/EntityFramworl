<template>
  <div>
    <h1>Liste des patients</h1>

    <div class="search-bar">
      <input
        v-model="searchQuery"
        @input="search"
        placeholder="Rechercher par nom..."
      />
    </div>

    <table v-if="patients.length > 0">
      <thead>
        <tr>
          <th>N° Dossier</th>
          <th>Nom</th>
          <th>Prénom</th>
          <th>Email</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="patient in patients" :key="patient.id">
          <td>{{ patient.fileNumber }}</td>
          <td>{{ patient.lastName }}</td>
          <td>{{ patient.firstName }}</td>
          <td>{{ patient.email }}</td>
          <td>
            <RouterLink :to="`/patients/${patient.id}`">Voir fiche</RouterLink>
          </td>
        </tr>
      </tbody>
    </table>

    <p v-else-if="!loading">Aucun patient trouvé.</p>
    <p v-else>Chargement...</p>

    <div class="pagination">
      <button @click="prevPage" :disabled="page === 1">← Précédent</button>
      <span>Page {{ page }}</span>
      <button @click="nextPage" :disabled="patients.length < pageSize">Suivant →</button>
    </div>
  </div>
</template>

<script>
import api from '../api/api'

export default {
  data() {
    return {
      patients: [],
      searchQuery: '',
      page: 1,
      pageSize: 10,
      loading: false
    }
  },
  mounted() {
    this.loadPatients()
  },
  methods: {
    async loadPatients() {
      this.loading = true
      try {
        const response = await api.get('/Patient', {
          params: { page: this.page, pageSize: this.pageSize }
        })
        this.patients = response.data
      } catch (e) {
        console.error(e)
      } finally {
        this.loading = false
      }
    },
    async search() {
      if (this.searchQuery.trim() === '') {
        this.loadPatients()
        return
      }
      const response = await api.get('/Patient/search', {
        params: { name: this.searchQuery }
      })
      this.patients = response.data
    },
    prevPage() {
      if (this.page > 1) { this.page--; this.loadPatients() }
    },
    nextPage() {
      this.page++; this.loadPatients()
    }
  }
}
</script>

<style scoped>
table { width: 100%; border-collapse: collapse; margin-top: 1rem; }
th, td { padding: 0.75rem; border: 1px solid #ddd; text-align: left; }
th { background: #2c3e50; color: white; }
tr:hover { background: #f5f5f5; }
.search-bar input {
  width: 300px; padding: 0.5rem;
  border: 1px solid #ddd; border-radius: 4px; font-size: 1rem;
}
.pagination { display: flex; align-items: center; gap: 1rem; margin-top: 1rem; }
button {
  padding: 0.5rem 1rem; background: #2c3e50;
  color: white; border: none; border-radius: 4px; cursor: pointer;
}
button:disabled { background: #ccc; cursor: not-allowed; }
a { color: #3498db; }
</style>