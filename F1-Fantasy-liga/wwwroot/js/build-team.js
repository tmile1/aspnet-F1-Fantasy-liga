(function () {
  const pageEl = document.querySelector(".build-team-page");
  if (!pageEl) return;

  // Config and State
  const budgetLimit =
    parseFloat(pageEl.getAttribute("data-budget-limit")) || 100.0;
  const maxDrivers = parseInt(pageEl.getAttribute("data-max-drivers")) || 4;

  let state = {
    selectedDrivers: [],
    usedBudget: 0.0,
  };

  // DOM Elements
  const usedBudgetEl = document.getElementById("usedBudget");
  const driverCountEl = document.getElementById("driverCount");
  const budgetProgressBarEl = document.getElementById("budgetProgressBar");
  const budgetProgressContainer = document.querySelector(".budget-progress");
  const driverMarketList = document.getElementById("driverMarketList");
  const selectedTeamList = document.getElementById("selectedTeamList");
  const emptyTeamHint = document.getElementById("emptyTeamHint");

  // Create a status message element
  const statusMsgContainer = document.createElement("div");
  statusMsgContainer.className = "status-message";
  statusMsgContainer.setAttribute("aria-live", "polite");
  pageEl.querySelector(".build-team-header").appendChild(statusMsgContainer);

  // Event Delegation
  driverMarketList.addEventListener("click", function (e) {
    if (e.target.classList.contains("add-driver-btn")) {
      const card = e.target.closest(".driver-card");
      handleAddDriver(card, e.target);
    }
  });

  selectedTeamList.addEventListener("click", function (e) {
    if (e.target.classList.contains("remove-driver-btn")) {
      const card = e.target.closest(".driver-card");
      handleRemoveDriver(card);
    }
  });

  function handleAddDriver(card, btn) {
    const driverId = card.getAttribute("data-driver-id");
    const price = parseFloat(card.getAttribute("data-price")) || 0;

    // Validation checks
    if (state.selectedDrivers.length >= maxDrivers) {
      showStatus("Maximum number of drivers reached (" + maxDrivers + ").");
      return;
    }

    if (state.usedBudget + price > budgetLimit) {
      showStatus("Adding this driver would exceed the budget limit.");
      return;
    }

    if (state.selectedDrivers.find((d) => d.id === driverId)) {
      return; // Already added
    }

    // Add to state
    state.selectedDrivers.push({ id: driverId, price: price });
    state.usedBudget += price;

    // UI Updates
    btn.disabled = true;
    btn.textContent = "Added";

    // Clone card for selected list
    const clonedCard = card.cloneNode(true);
    const actionBtn = clonedCard.querySelector("button");
    actionBtn.className = "driver-action-btn remove-driver-btn";
    actionBtn.textContent = "Remove";
    actionBtn.disabled = false;

    selectedTeamList.appendChild(clonedCard);

    updateUI();
    showStatus(""); // clear status
  }

  function handleRemoveDriver(card) {
    const driverId = card.getAttribute("data-driver-id");
    const price = parseFloat(card.getAttribute("data-price")) || 0;

    // Remove from state
    state.selectedDrivers = state.selectedDrivers.filter(
      (d) => d.id !== driverId,
    );
    state.usedBudget -= price;

    // Ensure we handle floating point precision issues safely
    if (state.selectedDrivers.length === 0 || state.usedBudget < 0) {
      state.usedBudget = 0.0;
    }

    // UI Updates
    card.remove();

    // Re-enable in market
    const marketCard = driverMarketList.querySelector(
      `.driver-card[data-driver-id="${driverId}"]`,
    );
    if (marketCard) {
      const btn = marketCard.querySelector("button");
      btn.disabled = false;
      btn.textContent = "Add";
    }

    updateUI();
    showStatus(""); // clear status
  }

  function updateUI() {
    // Update budget strings
    usedBudgetEl.textContent = state.usedBudget.toFixed(1) + " M";
    driverCountEl.textContent =
      state.selectedDrivers.length + " / " + maxDrivers;

    // Update Progress Bar
    const percentage = Math.min((state.usedBudget / budgetLimit) * 100, 100);
    budgetProgressBarEl.style.width = percentage + "%";
    budgetProgressContainer.setAttribute(
      "aria-valuenow",
      percentage.toFixed(0),
    );

    // Over-budget stylistic indication (if we ever support going over, but here we prevent it)
    if (percentage >= 100) {
      budgetProgressBarEl.classList.add("over-budget");
    } else {
      budgetProgressBarEl.classList.remove("over-budget");
    }

    // Empty state hint
    if (state.selectedDrivers.length === 0) {
      emptyTeamHint.style.display = "block";
    } else {
      emptyTeamHint.style.display = "none";
    }
  }

  function showStatus(msg) {
    statusMsgContainer.textContent = msg;
  }
})();
