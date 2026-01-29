<template>
  <q-layout view="lHh Lpr lFf">
    <q-header elevated>
      <q-toolbar>
        <q-toolbar-title class="column items-center text-weight-bold text-h3 q-my-lg">
          ShotSync Ranking
        </q-toolbar-title>
      </q-toolbar>
      <div style="position: absolute; top: 20px; right: 50px">
        <q-btn flat round dense icon="logout" class="q-mr-xs" @click="logout" />
        <q-btn flat round dense icon="account_circle" />
      </div>
      <q-tabs v-model="selectedTab">
        <NavBarComponent
          v-for="(tab, index) in tabs"
          :key="index"
          :tabName="tab.tabName"
          :tabLabel="tab.tabLabel"
          :tabDes="tab.tabDes"
          tabIcon=""
        />
      </q-tabs>
    </q-header>

    <!-- Confirmation Dialog -->
    <q-dialog v-model="confirmLogoutDialog" persistent>
      <q-card>
        <q-card-section>
          <div class="text-h6">Are you sure you want to leave now?</div>
        </q-card-section>

        <q-card-actions align="right">
          <q-btn flat label="Cancel" color="primary" v-close-popup />
          <q-btn flat label="Logout" color="negative" @click="confirmLogout" />
        </q-card-actions>
      </q-card>
    </q-dialog>

    <q-footer elevated>
      <q-toolbar>
        <q-toolbar-title class="column items-center text-caption">
          <span>Copyright <q-icon name="copyright" /> by shotsync.com</span>
        </q-toolbar-title>
      </q-toolbar>
    </q-footer>

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue'
import NavBarComponent from 'components/NavBarComponent.vue'
import { useRoute, useRouter } from 'vue-router'
import ROUTE_PATHS from 'src/router/route-paths'

const route = useRoute()
const router = useRouter()

const selectedTab = ref(route.path)
watch(route, (newRoute) => {
  selectedTab.value = newRoute.path
})

const tabs = ref([
  { tabName: ROUTE_PATHS.PLAYER.NEWS, tabLabel: 'News', tabDes: ROUTE_PATHS.PLAYER.NEWS },
  { tabName: ROUTE_PATHS.PLAYER.PLAYERS, tabLabel: 'Players', tabDes: ROUTE_PATHS.PLAYER.PLAYERS },
  {
    tabName: ROUTE_PATHS.PLAYER.RANKINGS,
    tabLabel: 'Rankings',
    tabDes: ROUTE_PATHS.PLAYER.RANKINGS,
  },
  { tabName: ROUTE_PATHS.PLAYER.EVENTS, tabLabel: 'Events', tabDes: ROUTE_PATHS.PLAYER.EVENTS },
])

const confirmLogoutDialog = ref(false)

const logout = () => {
  confirmLogoutDialog.value = true
}

const confirmLogout = async () => {
  localStorage.removeItem('Token')
  localStorage.removeItem('role')
  localStorage.removeItem('eventId')
  localStorage.removeItem('userInfor')
  localStorage.removeItem('userName')
  confirmLogoutDialog.value = false
  await router.push(ROUTE_PATHS.AUTH.LOGIN)
}
</script>
