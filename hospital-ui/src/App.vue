<template>
  <div>
    <nav>
      <RouterLink to="/patients">👤 Patients</RouterLink>
      <RouterLink to="/departments/stats">🏥 Départements</RouterLink>
      <RouterLink to="/doctor/1/planning">📅 Planning Médecin</RouterLink>
    </nav>
    <main>
      <RouterView />
    </main>
  </div>
</template>

<style scoped>
nav {
  background: #2c3e50;
  padding: 1rem 2rem;
  display: flex;
  gap: 2rem;
}

nav a {
  color: white;
  text-decoration: none;
  font-weight: bold;
}

nav a:hover {
  color: #3498db;
}

main {
  padding: 2rem;
}
</style>