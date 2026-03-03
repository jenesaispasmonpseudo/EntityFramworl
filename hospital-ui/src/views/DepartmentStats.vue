<template>
  <div>
    <h1>Statistiques des départements</h1>

    <table v-if="stats.length > 0">
      <thead>
        <tr>
          <th>Département</th>
          <th>Localisation</th>
          <th>Médecins</th>
          <th>Consultations</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="dept in stats" :key="dept.id">
          <td>{{ dept.name }}</td>
          <td>{{ dept.location }}</td>
          <td>{{ dept.doctorCount }}</td>
          <td>{{ dept.consultationCount }}</td>
        </tr>
      </tbody>
    </table>

    <p v-else>Chargement des statistiques...</p>
  </div>
</template>

<script>
import api from '../api/api'

export default {
  data() {
    return { stats: [] }
  },
  mounted() {
    this.loadStats()
  },
  methods: {
    async loadStats() {
      const response = await api.get('/Dashboard/departments/stats')
      this.stats = response.data
    }
  }
}
</script>

<style scoped>
table { width: 100%; border-collapse: collapse; margin-top: 1rem; }
th, td { padding: 0.75rem; border: 1px solid #ddd; text-align: left; }
th { background: #2c3e50; color: white; }
tr:hover { background: #f5f5f5; }
</style>