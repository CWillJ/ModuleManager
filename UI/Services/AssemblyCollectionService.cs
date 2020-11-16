﻿namespace ModuleManager.UI.Services
{
    using System.Collections.ObjectModel;
    using ModuleManager.ModuleObjects.Classes;
    using ModuleManager.UI.Interfaces;
    using Prism.Mvvm;

    /// <summary>
    /// Service providing concrete <see cref="IAssemblyCollectionService"/> implementations.
    /// </summary>
    public class AssemblyCollectionService : BindableBase, IAssemblyCollectionService
    {
        private ObservableCollection<AssemblyData> _assemblies;
        private object _selectedItem;
        private string _selectedItemName;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyCollectionService"/> class.
        /// </summary>
        public AssemblyCollectionService()
        {
            _assemblies = new ObservableCollection<AssemblyData>();
            _selectedItem = null;
            _selectedItemName = @"Description";
        }

        /// <summary>
        /// Gets or sets a collection of <see cref="AssemblyData"/> objects.
        /// </summary>
        public ObservableCollection<AssemblyData> Assemblies
        {
            get { return _assemblies; }
            set { SetProperty(ref _assemblies, value); }
        }

        /// <summary>
        /// Gets or sets the selected <see cref="object"/> from the tree.
        /// </summary>
        public object SelectedItem
        {
            get
            {
                return _selectedItem;
            }

            set
            {
                SetProperty(ref _selectedItem, value);
                SetSelectedItemName();
            }
        }

        /// <summary>
        /// Gets or sets name of the selected item.
        /// </summary>
        public string SelectedItemName
        {
            get { return _selectedItemName; }
            set { SetProperty(ref _selectedItemName, value); }
        }

        private void SetSelectedItemName()
        {
            if (SelectedItem is AssemblyData assembly)
            {
                SelectedItemName = assembly.Name;
            }
            else if (SelectedItem is ModuleData module)
            {
                SelectedItemName = module.Name;
            }
            else if (SelectedItem is ModuleMemberData member)
            {
                SelectedItemName = member.Name;
            }
            else
            {
                SelectedItemName = @"Description";
            }
        }
    }
}