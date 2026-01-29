<template>
  <q-tab
    :name="tabName"
    :label="tabLabel"
    @click="NavBarComponentHandle"
    :icon="tabIcon !== '' ? tabIcon : undefined"
  />
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router'
const router = useRouter()

export interface NavBarComponentProps {
  tabName: string
  tabLabel: string
  tabIcon: string
  tabDes: string
}

const props = withDefaults(defineProps<NavBarComponentProps>(), {
  tabName: 'newtab',
  tabDes: '#',
  tabLabel: 'New Tab',
  tabIcon: '',
})

async function NavBarComponentHandle() {
  await router.push(props.tabDes)
}
</script>
