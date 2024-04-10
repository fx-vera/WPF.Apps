using PropertyChanged;
using System.Collections.ObjectModel;
using Tucodev.ComicLand.Marvel.Models;

namespace Tucodev.ComicLand.Marvel.ViewModels
{
    /// <summary>
    /// Comics data
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class DisplayerModel
    {
        public DisplayerModel() { }

        public ObservableCollection<CreatorComicModel> CreatorComics { get; set; } = new ObservableCollection<CreatorComicModel>();
        public CreatorComicModel SelectedCreator { get; set; }
    }
}