// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

(() => {
  const isInteractiveTarget = (target) => {
    return !!target.closest("a, button, input, select, textarea, label");
  };

  const initAutocomplete = () => {
    const fields = document.querySelectorAll(".autocomplete-field");
    if (!fields.length) {
      return;
    }

    fields.forEach((field) => {
      const input = field.querySelector(".autocomplete-input");
      const hidden = field.querySelector("input[type='hidden']");
      const menu = field.querySelector(".autocomplete-menu");
      const searchUrl = field.dataset.autocompleteUrl;

      if (!input || !hidden || !menu || !searchUrl) {
        return;
      }

      let debounceTimer;

      const closeMenu = () => {
        menu.classList.remove("is-open");
        menu.innerHTML = "";
      };

      const openMenu = () => {
        if (menu.children.length > 0) {
          menu.classList.add("is-open");
        }
      };

      const renderResults = (items) => {
        menu.innerHTML = "";

        if (!items.length) {
          closeMenu();
          return;
        }

        items.forEach((item) => {
          const option = document.createElement("div");
          option.className = "autocomplete-item";
          option.textContent = item.label;
          option.dataset.id = item.id;
          option.dataset.label = item.label;
          menu.appendChild(option);
        });

        openMenu();
      };

      const runSearch = () => {
        const term = input.value.trim();
        const url = `${searchUrl}?term=${encodeURIComponent(term)}`;
        fetch(url, { headers: { "X-Requested-With": "XMLHttpRequest" } })
          .then((response) => response.json())
          .then((items) => {
            renderResults(items || []);
          })
          .catch(() => {
            closeMenu();
          });
      };

      input.addEventListener("input", () => {
        hidden.value = "";
        window.clearTimeout(debounceTimer);
        debounceTimer = window.setTimeout(runSearch, 250);
      });

      menu.addEventListener("click", (event) => {
        const option = event.target.closest(".autocomplete-item");
        if (!option) {
          return;
        }

        input.value = option.dataset.label || "";
        hidden.value = option.dataset.id || "";
        closeMenu();
      });

      document.addEventListener("click", (event) => {
        if (!field.contains(event.target)) {
          closeMenu();
        }
      });

      input.addEventListener("focus", () => {
        if (menu.children.length > 0) {
          openMenu();
        }
      });
    });
  };

  const initHomeCards = () => {
    const cards = document.querySelectorAll(".home-nav-grid .home-card");
    if (!cards.length) {
      return;
    }

    cards.forEach((card, index) => {
      window.setTimeout(() => {
        card.classList.add("is-visible");
      }, index * 90);
    });
  };

  const deleteModal = document.getElementById("delete-modal");
  const deleteForm = document.getElementById("delete-form");
  const deleteName = document.getElementById("delete-entity-name");

  const openDeleteModal = (trigger) => {
    if (!deleteModal || !deleteForm || !deleteName) {
      return;
    }

    deleteForm.setAttribute("action", trigger.dataset.deleteUrl || "");
    deleteName.textContent = trigger.dataset.entityName || "this item";
    deleteModal.classList.add("is-visible");
    deleteModal.setAttribute("aria-hidden", "false");
  };

  const closeDeleteModal = () => {
    if (!deleteModal) {
      return;
    }

    deleteModal.classList.remove("is-visible");
    deleteModal.setAttribute("aria-hidden", "true");
  };

  document.addEventListener("click", (event) => {
    const deleteTrigger = event.target.closest(".delete-trigger");
    if (deleteTrigger) {
      event.preventDefault();
      openDeleteModal(deleteTrigger);
      return;
    }

    if (
      event.target.matches("[data-modal-close]") ||
      event.target === deleteModal
    ) {
      event.preventDefault();
      closeDeleteModal();
      return;
    }

    const row = event.target.closest(".row-link[data-href]");
    if (!row || isInteractiveTarget(event.target)) {
      return;
    }

    window.location.href = row.dataset.href;
  });

  document.addEventListener("keydown", (event) => {
    if (event.key === "Escape") {
      closeDeleteModal();
      return;
    }

    const row = event.target.closest(".row-link[data-href]");
    if (!row || (event.key !== "Enter" && event.key !== " ")) {
      return;
    }

    event.preventDefault();
    window.location.href = row.dataset.href;
  });

  document.addEventListener("DOMContentLoaded", () => {
    initAutocomplete();
    initHomeCards();
  });
})();
