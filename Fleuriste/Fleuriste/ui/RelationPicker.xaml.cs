using BDD.Core;
using BDD.Core.Relations;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace BDD.UI
{
    public partial class RelationPicker : UserControl
    {
        public string Column { get; private set; }
        public ManyToManyRelation Relation { get; private set; }
        public EntityService EndService => Relation.Service.End;
        public bool Quantifiable => Relation.IsQuantifiable();
        public Func<Entity?>? CreateEndEntityFunc { get; private set; }

        public RelationPicker(ManyToManyRelation relation, string column, Func<Entity?>? createEndEntity=null)
        {
            InitializeComponent();
            Column = column;
            Relation = relation;
            CreateEndEntityFunc = createEndEntity;

            if (CreateEndEntityFunc == null)
                CreateEndEntity.Visibility = Visibility.Hidden;
            else
                CreateEndEntity.Click += delegate { CreateEndEntityAction(); };

            Dictionary<int, int> links = relation.DescribeId();
            RelationPickerItem item;

            foreach (Entity entity in EndService.SelectAll())
                if (links.ContainsKey(entity.Id))
                {
                    item = new(this, entity, true, links[entity.Id]);
                    LeftList.Items.Add(item);
                }
                else
                {
                    item = new(this, entity, false);
                    RightList.Items.Add(item);
                }

            ToLeftButton.Click += delegate { ToLeft(); };
            ToRightButton.Click += delegate { ToRight(); };
        }

        private void CreateEndEntityAction()
        {
            if (CreateEndEntityFunc == null)
                throw new ApplicationException();

            Entity? entity = CreateEndEntityFunc();
            if (entity == null)
                return;

            if (!entity.IsPersisted)
                try
                {
                    entity.Persist();
                }
                catch(MySqlException e)
                {
                    MessageBox.Show(e.Message, "Erreur");
                }

            RelationPickerItem item = new(this, entity, false);
            RightList.Items.Add(item);
        }
    
        private void ToLeft()
        {
            List<RelationPickerItem> toremove = new();

            foreach (RelationPickerItem selected in RightList.SelectedItems)
            {
                toremove.Add(selected);
                RelationPickerItem rpi = new(this, selected.Entity, true, 1);
                LeftList.Items.Add(rpi);
                Relation.Put(rpi.Entity.Id, 1);
            }

            toremove.ForEach(RightList.Items.Remove);
        }

        private void ToRight()
        {
            List<RelationPickerItem> toremove = new();

            foreach (RelationPickerItem selected in LeftList.SelectedItems)
            {
                toremove.Add(selected);
                RelationPickerItem rpi = new(this, selected.Entity, false);
                RightList.Items.Add(rpi);
                Relation.Remove(rpi.Entity.Id);
            }

            toremove.ForEach(LeftList.Items.Remove);
        }
    }
}
