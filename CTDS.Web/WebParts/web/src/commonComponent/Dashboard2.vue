<template>
  <div>
    <div class="grayColorBackground">
      <appNavigationbar></appNavigationbar>
      <b-card no-body>
        <b-tabs pills>
          <b-tab title="Cases" @click="showCases()"> </b-tab>
          <b-tab title="Declarations" @click="showDeclarations()"> </b-tab>
        </b-tabs>
        <component :is="selectedComponent"></component>
      </b-card>
    </div>
  </div>
</template>

<script>
import CaseDashboard from "./CaseDashboard2";
import DeclarationDashboard from "./DeclarationDashboard2";
import Navigationbar from "./Navigationbar";

export default {
  components: {
    appNavigationbar: Navigationbar
  },
  data() {
    return {
      selectedComponent: CaseDashboard
    };
  },
  methods: {
    showCases() {
      this.selectedComponent = CaseDashboard;
    },
    showDeclarations() {
      this.selectedComponent = DeclarationDashboard;
    }
  }
};
</script>

<style scoped>
@import url("https://fonts.googleapis.com/css?family=Source+Sans+Pro:200,400,900&display=swap");
@import url("./styles/dashboard2.css");
</style>
