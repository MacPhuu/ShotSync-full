<template>
  <q-layout class="bg-no-1 window-height window-width row justify-center items-center">
    <div class="column">
      <div class="row items-center justify-center">
        <div class="text-h3 text-secondary text-bold">ShotSync Ranking.</div>
      </div>
      <div class="row items-center justify-center">
        <div class="text-h6 text-white q-mb-lg">
          Welcome to the <span class="text-secondary">ShotSync Ranking.</span> live scoring.
        </div>
      </div>
      <div class="row">
        <q-form class="q-gutter-md" @submit.prevent="onSubmit">
          <q-card square bordered class="q-pa-md shadow-1">
            <q-card-section>
              <div class="text-h5 text-primary text-bold q-pb-md">Login</div>
              <q-input
                class="q-my-md"
                square
                filled
                clearable
                v-model="email"
                type="email"
                label="email"
              />
              <q-input
                class="q-my-md"
                square
                filled
                clearable
                v-model="password"
                type="password"
                label="password"
              />
              <q-checkbox v-model="rememberMe" label="Remember me" />
            </q-card-section>
            <q-card-actions class="q-px-md">
              <q-btn
                unelevated
                size="lg"
                class="full-width bg-primary text-white"
                type="submit"
                label="Login"
              />
            </q-card-actions>
            <q-card-section class="text-center q-pa-none">
              <p class="text-grey-6">Not registered? Create an account</p>
            </q-card-section>
          </q-card>
        </q-form>
      </div>
    </div>


  </q-layout>
</template>

<script lang="ts" setup>
import { ref } from 'vue'
import AuthService from 'src/services/auth.service'
import { useRouter } from 'vue-router'
import ROUTE_PATHS from 'src/router/route-paths'
import UserRole, * as UserRoleUtil from 'src/types/role.enum'
import { useQuasar } from 'quasar'

const router = useRouter()
const $q = useQuasar()

const email = ref('')
const password = ref('')
const rememberMe = ref(false)
const onSubmit = async () => {
  const loginInfor = {
    email: email.value,
    password: password.value,
  }

  try {
    const data = await AuthService.login(loginInfor)

    const token = data.token
    const role = data.role
    const userName = data.userName

    const storage = rememberMe.value ? localStorage : sessionStorage
    const otherStorage = rememberMe.value ? sessionStorage : localStorage

    // Clear other storage to avoid conflict
    otherStorage.removeItem('Token')
    otherStorage.removeItem('role')
    otherStorage.removeItem('userName')
    otherStorage.removeItem('userId')

    if (token) storage.setItem('Token', token)

    // Normalize role
    const normalizedRole = UserRoleUtil.parseUserRole(role)

    // Store normalized role number if available, else store original
    if (normalizedRole) {
      storage.setItem('role', String(normalizedRole))
    } else if (role) {
      storage.setItem('role', role)
    }

    if (userName) storage.setItem('userName', userName)
    if (data.id) storage.setItem('userId', String(data.id))

    const roleRoutes: Record<string, string> = {
      [UserRole.ADMIN]: ROUTE_PATHS.ADMIN.APP_STATUS,
      [UserRole.HOST]: ROUTE_PATHS.HOST.PROFILE,
      [UserRole.PLAYER]: ROUTE_PATHS.PLAYER.NEWS,
    }

    // Use normalized role for lookup
    // If normalizedRole is null, key lookup fails safely
    const targetRoute = (normalizedRole && roleRoutes[normalizedRole]) || ROUTE_PATHS.AUTH.LOGIN

    console.log('Role Raw:', role, 'Normalized:', normalizedRole, 'Target:', targetRoute)

    // Verify targetRoute is not login if we expect success (unless role is somehow invalid)
    if (targetRoute === ROUTE_PATHS.AUTH.LOGIN && normalizedRole) {
      console.error('Critical: Role normalized but no route found!')
    }

    await router.push(targetRoute)
    // Có thể thêm xử lý khác sau khi điều hướng thành công
  } catch (error) {
    console.error('Login failed:', error)
    $q.notify({
      color: 'negative',
      position: 'top',
      message: 'Invalid email or password',
      icon: 'report_problem',
    })
  }
}
</script>

<style>
.q-card {
  width: 450px;
}
</style>
